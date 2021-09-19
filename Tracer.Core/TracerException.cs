using System;

namespace Tracer.Core
{
    public class TracerException : Exception
    {
        public TracerException()
        {
        }

        public TracerException(string message) : base(message)
        {
        }

        public TracerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}