using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tracer.Core
{
    public sealed class ThreadTracer
    {
        private readonly List<MethodTracer> _methodsTracers;
        private readonly Stack<MethodTracer> _callStack;
        public IEnumerable<MethodTracer> MethodsTracers => _methodsTracers;
        public long ExecutionTime => _methodsTracers.Select(methodTracer => methodTracer.ExecutionTime).Sum();

        internal ThreadTracer()
        {
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