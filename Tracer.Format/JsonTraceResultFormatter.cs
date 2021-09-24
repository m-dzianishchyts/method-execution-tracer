using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Tracer.Core;

namespace Tracer
{
    namespace Format
    {
        public sealed class JsonTraceResultFormatter : ITraceResultFormatter
        {
            public void Format(Stream outputStream, TraceResult traceResult)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var root = new
                {
                    threads = traceResult.ThreadsTraceResults.Values.ToArray()
                };
                string json = JsonSerializer.Serialize(root, jsonSerializerOptions);
                outputStream.Write(Encoding.UTF8.GetBytes(json));
            }
        }
    }
}
