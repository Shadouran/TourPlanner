using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Logging
{
    public interface ILogger
    {
        void Debug(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
