using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tracer.Core;
using Tracer.Format;

namespace Tracer
{
    namespace Test
    {
        [TestFixture]
        public class TracerFormatTest
        {
            private const int DEFAULT_THREAD_SLEEP_TIMEOUT = 10;

            private readonly ITracer _tracer;

            public TracerFormatTest()
            {
                _tracer = new Core.Tracer();
            }

            [SetUp]
            public void Setup()
            {
                _tracer.Reset();
            }

            [Test]
            [TestCase("trace.json")]
            public void JsonFormat(string filePath)
            {
                GeneralTrace();
                FormatTraceResultToFile(filePath, new JsonTraceResultFormatter());
                FormatTraceResultToConsole(new JsonTraceResultFormatter());
                CheckJsonFile(filePath);
            }

            [Test]
            [TestCase("trace.xml")]
            public void XmlFormat(string filePath)
            {
                GeneralTrace();
                FormatTraceResultToFile(filePath, new XmlTraceResultFormatter());
                FormatTraceResultToConsole(new XmlTraceResultFormatter());
                CheckXmlFile(filePath);
            }

            private void FormatTraceResultToFile(string filePath, ITraceResultFormatter traceResultFormatter)
            {
                using Stream outputStream = new FileStream(filePath, FileMode.Create);
                TraceResult traceResult = _tracer.GetTraceResult();
                traceResultFormatter.Format(outputStream, traceResult);
            }

            private void FormatTraceResultToConsole(ITraceResultFormatter traceResultFormatter)
            {
                TraceResult traceResult = _tracer.GetTraceResult();
                traceResultFormatter.Format(TestContext.Out, traceResult);
            }

            private void GeneralTrace()
            {
                const int threadsAmount = 1;

                var threads = new List<Thread>();
                for (var i = 0; i < threadsAmount; i++)
                {
                    threads.Add(new Thread(NestedTracedDummyWithNestedCalls));
                }

                _tracer.StartTrace();
                threads.ForEach(thread => thread.Start());
                NestedTracedDummyWithoutNestedCalls();
                threads.ForEach(thread => thread.Join());
                _tracer.StopTrace();
            }

            private void NestedTracedDummyWithNestedCalls()
            {
                _tracer.StartTrace();
                Thread.Sleep(DEFAULT_THREAD_SLEEP_TIMEOUT);
                NestedTracedDummyWithoutNestedCalls();
                NestedTracedDummyWithoutNestedCalls();
                _tracer.StopTrace();
            }

            private void NestedTracedDummyWithoutNestedCalls()
            {
                _tracer.StartTrace();
                Thread.Sleep(DEFAULT_THREAD_SLEEP_TIMEOUT);
                _tracer.StopTrace();
            }

            private static void CheckJsonFile(string filePath)
            {
                FileAssert.Exists(filePath);

                void TestDelegate()
                {
                    using TextReader textReader = new StreamReader(new FileStream(filePath, FileMode.Open));
                    string json = textReader.ReadToEnd();
                    JToken.Parse(json);
                }

                Assert.DoesNotThrow(TestDelegate);
            }

            private static void CheckXmlFile(string filePath)
            {
                FileAssert.Exists(filePath);

                void TestDelegate()
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);
                }

                Assert.DoesNotThrow(TestDelegate);
            }
        }
    }
}
