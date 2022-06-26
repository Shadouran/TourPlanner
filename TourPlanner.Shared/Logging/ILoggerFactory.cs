﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger<TContext>();
    }
}
