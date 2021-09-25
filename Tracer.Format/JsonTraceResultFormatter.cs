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
                string json = FormatToJsonString(traceResult);
                outputStream.Write(Encoding.UTF8.GetBytes(json));
            }

            public void Format(TextWriter textWriter, TraceResult traceResult)
            {
                string json = FormatToJsonString(traceResult);
                textWriter.Write(json);
            }

            private static string FormatToJsonString(TraceResult traceResult)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var root = new
                {
                    threads = traceResult.ThreadTracers.Values.ToArray()
                };
                string json = JsonSerializer.Serialize(root, jsonSerializerOptions);
                return json;
            }
        }
    }
}
