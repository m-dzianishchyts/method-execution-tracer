namespace Tracer
{
    namespace Core
    {
        public interface ITracer
        {
            /// <summary>
            /// Starts tracing the method in which this <see cref="StartTrace"/> method is called.
            /// Tracking is performed within the current thread.
            /// </summary>
            void StartTrace();

            /// <summary>
            /// Finishes tracing the method in which this <see cref="StopTrace"/> method is called.
            /// Tracking is performed within the current thread.
            /// </summary>
            void StopTrace();

            /// <summary>
            /// Return the results of tracing the method in which <see cref="StartTrace"/>
            /// and <see cref="StopTrace"/> methods where called.
            /// In the case of multithreading use, the results are returned for each thread separately.
            /// <returns>Tracing results for each thread used in tracing.</returns>
            /// </summary>
            TraceResult GetTraceResult();
        }
    }
}
