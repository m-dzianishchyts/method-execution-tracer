﻿namespace Tracer.Core
{
    public interface ITracer
    {
        /// <summary>
        /// Starts tracing the method in which this <see cref="StartTrace"/> method is called.
        /// </summary>
        void StartTrace();

        /// <summary>
        /// Finishes tracing the method in which this <see cref="StopTrace"/> method is called.
        /// </summary>
        void StopTrace();

        /// <summary>
        /// Return the results of tracing the method in which <see cref="StartTrace"/>
        /// and <see cref="StopTrace"/> methods where called.
        /// </summary>
        TraceResult GetTraceResult();
    }
}