using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    namespace Core
    {
        public sealed class Tracer : ITracer
        {
            private const int INDEX_OF_PREVIOUS_STACK_FRAME = 1;

            private readonly ConcurrentDictionary<int, ThreadTracer> _threadsTraceResults = new();

            public void StartTrace()
            {
                var stackTrace = new StackTrace();
                StackFrame targetFrame = stackTrace.GetFrame(INDEX_OF_PREVIOUS_STACK_FRAME)!;
                MethodBase targetMethod = targetFrame.GetMethod()!;
                int threadId = DetermineCurrentThreadId();
                ThreadTracer threadTracer = _threadsTraceResults.GetOrAdd(threadId, new ThreadTracer(threadId));
                threadTracer.StartTrace(targetMethod);
            }

            private static int DetermineCurrentThreadId()
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                return threadId;
            }

            public void StopTrace()
            {
                ThreadTracer? threadsTraceResult;
                if (!_threadsTraceResults.TryGetValue(DetermineCurrentThreadId(), out threadsTraceResult))
                {
                    throw new ArgumentException("This thread is not being traced now.");
                }

                threadsTraceResult.StopTrace();
            }

            public TraceResult GetTraceResult()
            {
                return new TraceResult(_threadsTraceResults);
            }

            public void Reset()
            {
                _threadsTraceResults.Clear();
            }
        }
    }
}
