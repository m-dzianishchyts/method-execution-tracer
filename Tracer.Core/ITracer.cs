namespace Tracer
{
    namespace Core
    {
        public interface ITracer
        {
            /// <summary>
            /// Starts tracing the method in which this <see cref="StartTrace"/> method is called.
            /// <remarks>Tracking is performed within the current thread.</remarks>
            /// </summary>
            void StartTrace();

            /// <summary>
            /// Finishes tracing the method in which this <see cref="StopTrace"/> method is called.
            /// <remarks>Tracking is performed within the current thread.</remarks>
            /// </summary>
            void StopTrace();

            /// <summary>
            /// Return the results of tracing the method in which <see cref="StartTrace"/>
            /// and <see cref="StopTrace"/> methods where called.
            /// <remarks>In the case of multithreading use, the results are returned for each thread separately.</remarks>
            /// <returns>Tracing results for each thread used in tracing.</returns>
            /// </summary>
            TraceResult GetTraceResult();

            /// <summary>
            /// Reset tracing statistics.
            /// </summary>
            void Reset();
        }
    }
}
