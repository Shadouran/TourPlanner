using Npgsql;
using TourPlanner.Server.DAL.Records;

namespace TourPlanner.Server.DAL
{
    public class TourRepositoryPostgreSQL : ITourRepository
    {
        private const string CreateTablesQuery = "CREATE TABLE IF NOT EXISTS tours(id VARCHAR PRIMARY KEY, name VARCHAR, description VARCHAR, startlocation VARCHAR, targetlocation VARCHAR, transporttype VARCHAR, distance REAL, estimatedtime NUMERIC, routeinformation VARCHAR, imagefilename VARCHAR)";
        private const string CreateTourQuery = "INSERT INTO tours (id, name, description, startlocation, targetlocation, transporttype, distance, estimatedtime, routeinformation, imagefilename) VALUES (@id, @name, @description, @startlocation, @targetlocation, @transporttype, @distance, @estimatedtime, @routeinformation, @imagefilename)";
        private const string GetAllToursQuery = "SELECT * FROM tours";
        private const string GetTourByIdQuery = "SELECT * FROM tours WHERE id=@id";
        private const string GetMatchingsToursQuery = "SELECT * FROM tours WHERE ";
        private const string DeleteTourQuery = "DELETE FROM tours WHERE id=@id";
        private const string UpdateTourQuery = "UPDATE tours SET name=@name, description=@description, startlocation=@startlocation, targetlocation=@targetlocation, transporttype=@transporttype, distance=@distance, estimatedtime=@estimatedtime, routeinformation=@routeinformation, imagefilename=@imagefilename WHERE id=@id";
        
        private readonly NpgsqlConnection _connection;
        private readonly object _databaseLock;
        public TourRepositoryPostgreSQL(INpgsqlDatabase database)
        {
            _connection = database.Connection;
            _databaseLock = database.DatabaseLock;
            EnsureTables();
        }

        public void Create(Tour tour)
        {
            using var cmd = new NpgsqlCommand(CreateTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", tour.Id.ToString());
            cmd.Parameters.AddWithValue("name", tour.Name);
            cmd.Parameters.AddWithValue("description", tour.Description);
            cmd.Parameters.AddWithValue("startlocation", tour.StartLocation);
            cmd.Parameters.AddWithValue("targetlocation", tour.TargetLocation);
            cmd.Parameters.AddWithValue("transporttype", tour.TransportType);
            cmd.Parameters.AddWithValue("distance", tour.Distance);
            cmd.Parameters.AddWithValue("estimatedtime", tour.EstimatedTime);
            cmd.Parameters.AddWithValue("routeinformation", tour.RouteInformation);
            cmd.Parameters.AddWithValue("imagefilename", tour.ImageFileName);
            lock (_databaseLock)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public async Task CreateAsync(Tour tour)
        {
            using var cmd = new NpgsqlCommand(CreateTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", tour.Id.ToString());
            cmd.Parameters.AddWithValue("name", tour.Name);
            cmd.Parameters.AddWithValue("description", tour.Description);
            cmd.Parameters.AddWithValue("startlocation", tour.StartLocation);
            cmd.Parameters.AddWithValue("targetlocation", tour.TargetLocation);
            cmd.Parameters.AddWithValue("transporttype", tour.TransportType);
            cmd.Parameters.AddWithValue("distance", tour.Distance);
            cmd.Parameters.AddWithValue("estimatedtime", tour.EstimatedTime);
            cmd.Parameters.AddWithValue("routeinformation", tour.RouteInformation);
            cmd.Parameters.AddWithValue("imagefilename", tour.ImageFileName);
            await cmd.ExecuteNonQueryAsync();
        }

        public void Delete(Guid id)
        {
            using var cmd = new NpgsqlCommand(DeleteTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", id.ToString());
            lock(_databaseLock)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var cmd = new NpgsqlCommand(DeleteTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", id.ToString());
            await cmd.ExecuteNonQueryAsync();
        }

        public IEnumerable<Tour> GetAll()
        {
            using var cmd = new NpgsqlCommand(GetAllToursQuery, _connection);
            var result = new List<Tour>();
            lock(_databaseLock)
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tour = new Tour(Guid.Parse(reader.GetString(reader.GetOrdinal("id"))),
                                            reader.GetString(reader.GetOrdinal("name")),
                                            reader.GetString(reader.GetOrdinal("description")),
                                            reader.GetString(reader.GetOrdinal("startlocation")),
                                            reader.GetString(reader.GetOrdinal("targetlocation")),
                                            reader.GetString(reader.GetOrdinal("transporttype")),
                                            reader.GetString(reader.GetOrdinal("routeinformation")),
                                            reader.GetFloat(reader.GetOrdinal("distance")),
                                            reader.GetInt32(reader.GetOrdinal("estimatedtime")),
                                            Guid.Parse(reader.GetString(reader.GetOrdinal("imagefilename"))));
                    result.Add(tour);
                }
                reader.Close();
            }
            return result;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            using var cmd = new NpgsqlCommand(GetAllToursQuery, _connection);
            var result = new List<Tour>();
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tour = new Tour(Guid.Parse(reader.GetString(reader.GetOrdinal("id"))),
                                            reader.GetString(reader.GetOrdinal("name")),
                                            reader.GetString(reader.GetOrdinal("description")),
                                            reader.GetString(reader.GetOrdinal("startlocation")),
                                            reader.GetString(reader.GetOrdinal("targetlocation")),
                                            reader.GetString(reader.GetOrdinal("transporttype")),
                                            reader.GetString(reader.GetOrdinal("routeinformation")),
                                            reader.GetFloat(reader.GetOrdinal("distance")),
                                            reader.GetInt32(reader.GetOrdinal("estimatedtime")),
                                            Guid.Parse(reader.GetString(reader.GetOrdinal("imagefilename"))));
                result.Add(tour);
            }
            await reader.CloseAsync();
            return result;
        }

        public Tour? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand(GetTourByIdQuery, _connection);
            cmd.Parameters.AddWithValue("id", id.ToString());
            Tour? result = null;
            lock(_databaseLock)
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = new Tour(Guid.Parse(reader.GetString(reader.GetOrdinal("id"))),
                                            reader.GetString(reader.GetOrdinal("name")),
                                            reader.GetString(reader.GetOrdinal("description")),
                                            reader.GetString(reader.GetOrdinal("startlocation")),
                                            reader.GetString(reader.GetOrdinal("targetlocation")),
                                            reader.GetString(reader.GetOrdinal("transporttype")),
                                            reader.GetString(reader.GetOrdinal("routeinformation")),
                                            reader.GetFloat(reader.GetOrdinal("distance")),
                                            reader.GetInt32(reader.GetOrdinal("estimatedtime")),
                                            Guid.Parse(reader.GetString(reader.GetOrdinal("imagefilename"))));
                }
                reader.Close();
            }
            return result;
        }

        public async Task<Tour?> GetByIdAsync(Guid id)
        {
            using var cmd = new NpgsqlCommand(GetTourByIdQuery, _connection);
            Tour? result = null;
            cmd.Parameters.AddWithValue("id", id.ToString());
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result = new Tour(Guid.Parse(reader.GetString(reader.GetOrdinal("id"))),
                                            reader.GetString(reader.GetOrdinal("name")),
                                            reader.GetString(reader.GetOrdinal("description")),
                                            reader.GetString(reader.GetOrdinal("startlocation")),
                                            reader.GetString(reader.GetOrdinal("targetlocation")),
                                            reader.GetString(reader.GetOrdinal("transporttype")),
                                            reader.GetString(reader.GetOrdinal("routeinformation")),
                                            reader.GetFloat(reader.GetOrdinal("distance")),
                                            reader.GetInt32(reader.GetOrdinal("estimatedtime")),
                                            Guid.Parse(reader.GetString(reader.GetOrdinal("imagefilename"))));
            }
            await reader.CloseAsync();
            return result;
        }

        public Task<IEnumerable<Tour>> GetMatchingAsync(string searchText)
        {
            throw new NotImplementedException();
        }

        public void Update(Tour tour)
        {
            using var cmd = new NpgsqlCommand(UpdateTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", tour.Id.ToString());
            cmd.Parameters.AddWithValue("name", tour.Name);
            cmd.Parameters.AddWithValue("description", tour.Description);
            cmd.Parameters.AddWithValue("startlocation", tour.StartLocation);
            cmd.Parameters.AddWithValue("targetlocation", tour.TargetLocation);
            cmd.Parameters.AddWithValue("transporttype", tour.TransportType);
            cmd.Parameters.AddWithValue("distance", tour.Distance);
            cmd.Parameters.AddWithValue("estimatedtime", tour.EstimatedTime);
            cmd.Parameters.AddWithValue("routeinformation", tour.RouteInformation);
            cmd.Parameters.AddWithValue("imagefilename", tour.ImageFileName);
            lock (_databaseLock)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public async Task UpdateAsync(Tour tour)
        {
            using var cmd = new NpgsqlCommand(UpdateTourQuery, _connection);
            cmd.Parameters.AddWithValue("id", tour.Id.ToString());
            cmd.Parameters.AddWithValue("name", tour.Name);
            cmd.Parameters.AddWithValue("description", tour.Description);
            cmd.Parameters.AddWithValue("startlocation", tour.StartLocation);
            cmd.Parameters.AddWithValue("targetlocation", tour.TargetLocation);
            cmd.Parameters.AddWithValue("transporttype", tour.TransportType);
            cmd.Parameters.AddWithValue("distance", tour.Distance);
            cmd.Parameters.AddWithValue("estimatedtime", tour.EstimatedTime);
            cmd.Parameters.AddWithValue("routeinformation", tour.RouteInformation);
            cmd.Parameters.AddWithValue("imagefilename", tour.ImageFileName);
            await cmd.ExecuteNonQueryAsync();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTablesQuery, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}
