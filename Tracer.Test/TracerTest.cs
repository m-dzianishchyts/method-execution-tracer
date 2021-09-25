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
                const int threadSleepTimeout = 10;
                const int threadsAmount = 1;
                const int methodsAmount = 1;
                const int nestedMethodsAmount = 0;

                _tracer.StartTrace();
                Thread.Sleep(threadSleepTimeout);
                _tracer.StopTrace();

                TraceResult traceResult = _tracer.GetTraceResult();
                ImmutableDictionary<string, ThreadTracer> threadTracers = traceResult.ThreadsTraceResults;
                Assert.AreEqual(threadTracers.Count, threadsAmount);

                ThreadTracer threadTracer = threadTracers.Values.First();
                Assert.Greater(threadTracer.ExecutionTime, threadSleepTimeout);
                Assert.AreEqual(threadTracer.MethodsTracers.Count(), methodsAmount);
                if (Thread.CurrentThread.Name != null)
                    Assert.AreEqual(threadTracer.ThreadName, Thread.CurrentThread.Name);
                else
                    Assert.AreEqual(threadTracer.ThreadName, Thread.CurrentThread.ManagedThreadId);

                MethodTracer methodTracer = threadTracer.MethodsTracers.First();
                Assert.AreEqual(methodTracer.ExecutionTime, threadTracer.ExecutionTime);
                Assert.AreEqual(methodTracer.TypeName, nameof(TracerTest));
                Assert.AreEqual(methodTracer.MethodName, nameof(BasicTrace));
                Assert.AreEqual(methodTracer.NestedMethodsTracers.Count(), nestedMethodsAmount);
            }
        }
    }
}
