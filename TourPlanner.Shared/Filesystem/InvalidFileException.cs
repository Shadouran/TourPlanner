﻿using System.Runtime.Serialization;

namespace TourPlanner.Shared.Filesystem
{
    [Serializable]
    internal class InvalidFileException : Exception
    {
        public InvalidFileException()
        {
        }

        public InvalidFileException(string? message) : base(message)
        {
        }

        public InvalidFileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}