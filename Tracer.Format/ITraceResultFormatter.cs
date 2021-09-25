using System.IO;
using Tracer.Core;

namespace Tracer
{
    namespace Format
    {
        public interface ITraceResultFormatter
        {
            void Format(Stream outputStream, TraceResult traceResult);
            
            void Format(TextWriter textWriter, TraceResult traceResult);
        }
    }
}