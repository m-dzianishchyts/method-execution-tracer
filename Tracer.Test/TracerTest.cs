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
                ImmutableDictionary<string, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(threadTracer.MethodsTracers.Count(), methodsAmount);
                AssertThreadName(threadTracer);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                AssertMethodTracer(methodTracer, nameof(TracerTest), nameof(BasicTrace),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmount);

                Assert.AreEqual(threadTracer.ExecutionTime, methodTracer.ExecutionTime);
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
                ImmutableDictionary<string, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(threadTracer.MethodsTracers.Count(), methodsAmount);
                AssertThreadName(threadTracer);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                AssertMethodTracer(methodTracer, nameof(TracerTest), nameof(OneNestedMethodTrace),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmount);

                MethodTracer nestedMethodTracer = methodTracer.NestedMethodTracers.First();
                AssertMethodTracer(nestedMethodTracer, nameof(TracerTest), nameof(NestedTracedDummy),
                                   DEFAULT_THREAD_SLEEP_TIMEOUT, nestedMethodsAmountInNestedMethod);

                Assert.AreEqual(threadTracer.ExecutionTime, methodTracer.ExecutionTime);
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
                ImmutableDictionary<string, ThreadTracer> threadTracers = traceResult.ThreadTracers;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.AreEqual(threadTracer.MethodsTracers.Count(), methodsAmount);
                AssertThreadName(threadTracer);

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
                Assert.AreEqual(methodTracer.TypeName, expectedClassName);
                Assert.AreEqual(methodTracer.MethodName, expectedMethodName);
                Assert.Greater(methodTracer.ExecutionTime, expectedExecutionTime);
                Assert.AreEqual(methodTracer.NestedMethodTracers.Count(), expectedNestedMethodsAmount);
            }

            private static void AssertThreadName(ThreadTracer threadTracer)
            {
                if (Thread.CurrentThread.Name != null)
                    Assert.AreEqual(threadTracer.ThreadName, Thread.CurrentThread.Name);
                else
                    Assert.AreEqual(threadTracer.ThreadName, Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
