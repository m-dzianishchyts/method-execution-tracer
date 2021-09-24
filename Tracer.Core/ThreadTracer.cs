using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Tracer.Util;

namespace Tracer
{
    namespace Core
    {
        public sealed class ThreadTracer
        {
            private readonly List<MethodTracer> _methodsTracers;
            private readonly Stack<MethodTracer> _callStack;

            [JsonPropertyName("name")]
            public string ThreadName { get; }

            [JsonPropertyName("time")]
            [JsonConverter(typeof(JsonTimeMsConverter))]
            public long ExecutionTime => _methodsTracers.Select(methodTracer => methodTracer.ExecutionTime).Sum();

            [JsonPropertyName("methods")]
            public IEnumerable<MethodTracer> MethodsTracers => _methodsTracers;

            internal ThreadTracer(string threadName)
            {
                ThreadName = threadName;
                _callStack = new Stack<MethodTracer>();
                _methodsTracers = new List<MethodTracer>();
            }

            internal void StartTrace(MethodBase method)
            {
                var methodTraceResult = new MethodTracer(method);
                if (_callStack.Count == 0)
                {
                    _methodsTracers.Add(methodTraceResult);
                }
                else
                {
                    _callStack.Peek().AddNestedMethod(methodTraceResult);
                }

                _callStack.Push(methodTraceResult);
                methodTraceResult.StartTrace();
            }

            internal void StopTrace()
            {
                if (_callStack.Count == 0)
                {
                    throw new InvalidOperationException("Call stack has no methods to stop tracing.");
                }

                _callStack.Peek().StopTrace();
                _callStack.Pop();
            }
        }
    }
}
