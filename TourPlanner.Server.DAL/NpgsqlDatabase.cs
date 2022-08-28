using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Server.DAL
{
    public class NpgsqlDatabase : INpgsqlDatabase
    {
        private readonly Semaphore _databaseLock = new(1,1);
        public Semaphore DatabaseLock { get => _databaseLock;  }

        private readonly NpgsqlConnection _connection;
        public NpgsqlConnection Connection { get => _connection;  }

        private const string ClearDatabaseQuery = "DROP SCHEMA PUBLIC CASCADE; CREATE SCHEMA PUBLIC;";

        public NpgsqlDatabase(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration.GetConnectionString("Default"));
            _connection.Open();
            //ClearDatabase();
        }

        private void ClearDatabase()
        {
            using var cmd = new NpgsqlCommand(ClearDatabaseQuery, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}
