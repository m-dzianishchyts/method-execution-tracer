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

            private readonly ConcurrentDictionary<string, ThreadTracer> _threadsTraceResults = new();

            public void StartTrace()
            {
                var stackTrace = new StackTrace();
                StackFrame targetFrame = stackTrace.GetFrame(INDEX_OF_PREVIOUS_STACK_FRAME)!;
                MethodBase targetMethod = targetFrame.GetMethod()!;
                string threadName = DetermineCurrentThreadName();
                ThreadTracer threadTracer = _threadsTraceResults.GetOrAdd(threadName, new ThreadTracer(threadName));
                threadTracer.StartTrace(targetMethod);
            }

            private static string DetermineCurrentThreadName()
            {
                Thread currentThread = Thread.CurrentThread;
                string threadName = currentThread.Name ?? currentThread.ManagedThreadId.ToString();
                return threadName;
            }

            public void StopTrace()
            {
                ThreadTracer? threadsTraceResult;
                if (!_threadsTraceResults.TryGetValue(DetermineCurrentThreadName(), out threadsTraceResult))
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
