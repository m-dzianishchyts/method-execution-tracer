using System.IO;
using Tracer.Core;

namespace Tracer
{
    namespace Format
    {
        public interface ITraceResultFormatter
        {
            void Format(Stream outputStream, TraceResult traceResult);
        }
    }
}