using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Server.DAL.Records;
using TourPlanner.Shared.Models;
using TourLog = TourPlanner.Server.DAL.Records.TourLog;

namespace TourPlanner.Server.DAL.Repositories
{
    public class LogRepositoryPostgreSQL : ILogRepository
    {
        private const string CreateTablesQuery = "CREATE TABLE IF NOT EXISTS logs(id VARCHAR PRIMARY KEY, tourId VARCHAR, date VARCHAR, time VARCHAR, totalTime VARCHAR, difficulty VARCHAR, rating NUMERIC, comment VARCHAR, CONSTRAINT fk_tourId FOREIGN KEY (tourId) REFERENCES tours(id) ON DELETE CASCADE)";
        private const string GetAllLogsQuery = "SELECT * FROM logs WHERE tourId=@tourId";
        private const string CreateLogQuery = "INSERT INTO logs (id, tourId, date, time, totalTime, difficulty, rating, comment) VALUES (@id, @tourId, @date, @time, @totalTime, @difficulty, @rating, @comment)";
        private const string UpdateLogQuery = "UPDATE logs SET date=@date, time=@time, totalTime=@totalTime, difficulty=@difficulty, rating=@rating, comment=@comment WHERE id=@id";
        private const string DeleteLogQuery = "DELETE FROM logs WHERE id=@id";

        private readonly NpgsqlConnection _connection;
        private readonly Semaphore _databaseLock;
        public LogRepositoryPostgreSQL(INpgsqlDatabase database)
        {
            _connection = database.Connection;
            _databaseLock = database.DatabaseLock;
            EnsureTables();
        }

        public async Task<IEnumerable<TourLog>> GetAllLogsAsync(Guid tourId)
        {
            using var cmd = new NpgsqlCommand(GetAllLogsQuery, _connection);
            cmd.Parameters.AddWithValue("tourId", tourId.ToString());
            var result = new List<TourLog>();
            _databaseLock.WaitOne();
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tour = new TourLog(Guid.Parse(reader.GetString(reader.GetOrdinal("id"))),
                                            DateTime.Parse(reader.GetString(reader.GetOrdinal("date"))),
                                            reader.GetString(reader.GetOrdinal("time")),
                                            reader.GetString(reader.GetOrdinal("totalTime")),
                                            Enum.Parse<Difficulty>(reader.GetString(reader.GetOrdinal("difficulty"))),
                                            reader.GetInt32(reader.GetOrdinal("rating")),
                                            reader.GetString(reader.GetOrdinal("comment")));
                result.Add(tour);
            }
            await reader.CloseAsync();
            _databaseLock.Release();
            return result;
        }

        public async Task CreateAsync(TourLog log, Guid tourId)
        {
            using var cmd = new NpgsqlCommand(CreateLogQuery, _connection);
            cmd.Parameters.AddWithValue("id", log.Id.ToString());
            cmd.Parameters.AddWithValue("tourId", tourId.ToString());
            cmd.Parameters.AddWithValue("date", log.Date.ToString("dd.MM.yyyy"));
            cmd.Parameters.AddWithValue("time", log.Time);
            cmd.Parameters.AddWithValue("totalTime", log.TotalTime);
            cmd.Parameters.AddWithValue("difficulty", log.Difficulty.ToString());
            cmd.Parameters.AddWithValue("rating", log.Rating);
            cmd.Parameters.AddWithValue("comment", log.Comment);
            _databaseLock.WaitOne();
            await cmd.ExecuteNonQueryAsync();
            _databaseLock.Release();
        }

        public async Task UpdateAsync(TourLog log)
        {
            using var cmd = new NpgsqlCommand(UpdateLogQuery, _connection);
            cmd.Parameters.AddWithValue("id", log.Id.ToString());
            cmd.Parameters.AddWithValue("date", log.Date.ToString("dd.MM.yyyy"));
            cmd.Parameters.AddWithValue("time", log.Time);
            cmd.Parameters.AddWithValue("totalTime", log.TotalTime);
            cmd.Parameters.AddWithValue("difficulty", log.Difficulty.ToString());
            cmd.Parameters.AddWithValue("rating", log.Rating);
            cmd.Parameters.AddWithValue("comment", log.Comment);
            _databaseLock.WaitOne();
            await cmd.ExecuteNonQueryAsync();
            _databaseLock.Release();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var cmd = new NpgsqlCommand(DeleteLogQuery, _connection);
            cmd.Parameters.AddWithValue("id", id.ToString());
            _databaseLock.WaitOne();
            await cmd.ExecuteNonQueryAsync();
            _databaseLock.Release();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTablesQuery, _connection);
            _databaseLock.WaitOne();
            cmd.ExecuteNonQuery();
            _databaseLock.Release();
        }
    }
}
