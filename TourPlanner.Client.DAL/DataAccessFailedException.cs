﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JourneyDown.DAL
{
    public class DataAccessFailedException : Exception
    {
        public DataAccessFailedException()
        {
        }

        public DataAccessFailedException(string? message) : base(message)
        {
        }

        public DataAccessFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
