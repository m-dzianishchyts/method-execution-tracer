using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer.Core
{
    public sealed class Tracer : ITracer
    {
        private readonly ConcurrentDictionary<int, ThreadTracer> _threadsTraceResults;

        public Tracer()
        {
            _threadsTraceResults = new ConcurrentDictionary<int, ThreadTracer>();
        }

        public void StartTrace()
        {
            var stackTrace = new StackTrace();
            StackFrame targetFrame = stackTrace.GetFrame(1)!;
            MethodBase targetMethod = targetFrame.GetMethod()!;
            ThreadTracer threadTracer =
                _threadsTraceResults.GetOrAdd(Thread.CurrentThread.ManagedThreadId, new ThreadTracer());
            threadTracer.StartTrace(targetMethod);
        }

        public void StopTrace()
        {
            ThreadTracer? threadsTraceResult;
            if (!_threadsTraceResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out threadsTraceResult))
            {
                throw new ArgumentException("This thread is not being traced now.");
            }
            threadsTraceResult.StopTrace();
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_threadsTraceResults);
        }
    }
}