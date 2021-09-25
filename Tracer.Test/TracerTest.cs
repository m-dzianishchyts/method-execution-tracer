using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Tracer.Core;

namespace Tracer
{
    namespace Test
    {
        [TestFixture]
        public class TracerTest
        {
            private const int DEFAULT_THREAD_SLEEP_TIMEOUT = 10;

            private readonly ITracer _tracer;

            public TracerTest()
            {
                _tracer = new Core.Tracer();
            }

            [SetUp]
            public void Setup()
            {
                _tracer.Reset();
            }

            [Test]
            public void BasicTrace()
            {
                const int threadsAmount = 1;
                const int methodsAmount = 1;
                const int nestedMethodsAmount = 0;

                _tracer.StartTrace();
                Thread.Sleep(DEFAULT_THREAD_SLEEP_TIMEOUT);
                _tracer.StopTrace();

                TraceResult traceResult = _tracer.GetTraceResult();
                ImmutableDictionary<int, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(methodsAmount, threadTracer.MethodsTracers.Count());
                Assert.AreEqual(threadTracer.ThreadId, Thread.CurrentThread.ManagedThreadId);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                AssertMethodTracer(methodTracer, nameof(TracerTest), nameof(BasicTrace),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmount);

                Assert.AreEqual(methodTracer.ExecutionTime, threadTracer.ExecutionTime);
            }

            [Test]
            public void OneNestedMethodTrace()
            {
                const int threadsAmount = 1;
                const int methodsAmount = 1;
                const int nestedMethodsAmount = 1;
                const int nestedMethodsAmountInNestedMethod = 0;

                _tracer.StartTrace();
                NestedTracedDummy();
                _tracer.StopTrace();

                TraceResult traceResult = _tracer.GetTraceResult();
                ImmutableDictionary<int, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(methodsAmount, threadTracer.MethodsTracers.Count());
                Assert.AreEqual(threadTracer.ThreadId, Thread.CurrentThread.ManagedThreadId);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                AssertMethodTracer(methodTracer, nameof(TracerTest), nameof(OneNestedMethodTrace),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmount);

                MethodTracer nestedMethodTracer = methodTracer.NestedMethodTracers.First();
                AssertMethodTracer(nestedMethodTracer, nameof(TracerTest), nameof(NestedTracedDummy),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmountInNestedMethod);

                Assert.AreEqual(methodTracer.ExecutionTime, threadTracer.ExecutionTime);
            }

            [Test]
            public void SeveralNestedMethodTrace()
            {
                const int threadsAmount = 1;
                const int methodsAmount = 1;
                const int nestedMethodsAmount = 2;
                const int nestedMethodsAmountInNestedMethod = 0;

                _tracer.StartTrace();
                NestedTracedDummy();
                NestedTracedDummy();
                _tracer.StopTrace();

                TraceResult traceResult = _tracer.GetTraceResult();
                ImmutableDictionary<int, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadsAmount, threadTracers.Count);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(methodsAmount, threadTracer.MethodsTracers.Count());
                Assert.AreEqual(threadTracer.ThreadId, Thread.CurrentThread.ManagedThreadId);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                AssertMethodTracer(methodTracer, nameof(TracerTest), nameof(SeveralNestedMethodTrace),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmount);

                foreach (MethodTracer nestedMethodTracer in methodTracer.NestedMethodTracers)
                {
                    AssertMethodTracer(nestedMethodTracer, nameof(TracerTest), nameof(NestedTracedDummy),
                                       DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmountInNestedMethod);
                }

                Assert.AreEqual(threadTracer.ExecutionTime, methodTracer.ExecutionTime);
            }

            private void NestedTracedDummy()
            {
                _tracer.StartTrace();
                Thread.Sleep(DEFAULT_THREAD_SLEEP_TIMEOUT);
                _tracer.StopTrace();
            }

            private static void AssertMethodTracer(MethodTracer methodTracer, string expectedClassName,
                                                   string expectedMethodName,
                                                   int expectedExecutionTime, int expectedNestedMethodsAmount)
            {
                Assert.AreEqual(expectedClassName, methodTracer.TypeName);
                Assert.AreEqual(expectedMethodName, methodTracer.MethodName);
                Assert.GreaterOrEqual(methodTracer.ExecutionTime, expectedExecutionTime);
                Assert.AreEqual(expectedNestedMethodsAmount, methodTracer.NestedMethodTracers.Count());
            }
        }
    }
}
