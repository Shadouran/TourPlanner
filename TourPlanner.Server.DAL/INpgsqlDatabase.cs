using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Server.DAL
{
    public interface INpgsqlDatabase
    {
        NpgsqlConnection Connection {get;}

        object DatabaseLock { get; }
    }
}
