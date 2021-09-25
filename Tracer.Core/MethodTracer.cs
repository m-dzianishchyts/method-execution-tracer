using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using Tracer.Format;

namespace Tracer
{
    namespace Core
    {
        public sealed class MethodTracer
        {
            private readonly IList<MethodTracer> _nestedMethodsTracers;
            private readonly Stopwatch _stopwatch;

            [JsonPropertyName(SerializationConfig.JSON_METHOD_CLASS)]
            public string TypeName { get; }

            [JsonPropertyName(SerializationConfig.JSON_METHOD_NAME)]
            public string MethodName { get; }

            [JsonPropertyName(SerializationConfig.JSON_METHOD_TIME)]
            [JsonConverter(typeof(JsonTimeMsConverter))]
            public long ExecutionTime => _stopwatch.ElapsedMilliseconds;

            [JsonPropertyName(SerializationConfig.JSON_METHOD_INNER_METHODS)]
            public IEnumerable<MethodTracer> NestedMethodsTracers => _nestedMethodsTracers;

            internal MethodTracer(MethodBase method)
            {
                MethodName = method.Name;
                Type declaringType = method.DeclaringType ??
                                     throw new ArgumentException($"{nameof(method)} has no declaring type.");
                TypeName = declaringType.Name;
                _stopwatch = new Stopwatch();
                _nestedMethodsTracers = new List<MethodTracer>();
            }

            internal void StartTrace()
            {
                _stopwatch.Restart();
            }

            internal void StopTrace()
            {
                _stopwatch.Stop();
            }

            internal void AddNestedMethod(MethodTracer methodTracer)
            {
                _nestedMethodsTracers.Add(methodTracer);
            }
        }
    }
}
