using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Logging
{
    public static class LogManager
    {
        public static ILoggerFactory LoggerFactory { get; set; }

        public static ILogger GetLogger<TContext>()
        {
            return LoggerFactory.CreateLogger<TContext>();
        }
    }
}
