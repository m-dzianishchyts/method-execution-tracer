using System.IO;
using System.Xml;
using System.Xml.Linq;
using Tracer.Core;
using Tracer.Util;

namespace Tracer
{
    namespace Format
    {
        public sealed class XmlTraceResultFormatter : ITraceResultFormatter
        {
            public void Format(Stream outputStream, TraceResult traceResult)
            {
                var xDocument = new XDocument();
                var rootElement = new XElement("root");
                foreach (ThreadTracer threadTracer in traceResult.ThreadsTraceResults.Values)
                {
                    XElement threadElement = ConvertThreadTracerToXElement(threadTracer);
                    rootElement.Add(threadElement);
                }

                xDocument.Add(rootElement);
                XmlWriterSettings xws = new() {OmitXmlDeclaration = true, Indent = true};
                using XmlWriter xmlWriter = XmlWriter.Create(outputStream, xws);
                xDocument.Save(xmlWriter);
            }

            private static XElement ConvertThreadTracerToXElement(ThreadTracer threadTracer)
            {
                var threadElement = new XElement("thread");
                threadElement.Add(new XAttribute("id", threadTracer.ThreadName));
                threadElement.Add(new XAttribute("time",
                                                 TimeFormatUtil.FormatMilliseconds(threadTracer.ExecutionTime)));
                foreach (MethodTracer methodTracer in threadTracer.MethodsTracers)
                {
                    XElement methodElement = ConvertMethodTracerToXElement(methodTracer);
                    threadElement.Add(methodElement);
                }

                return threadElement;
            }

            private static XElement ConvertMethodTracerToXElement(MethodTracer methodTraceInfo)
            {
                var xElement = new XElement("method");
                xElement.Add(new XAttribute("class", methodTraceInfo.TypeName));
                xElement.Add(new XAttribute("name", methodTraceInfo.MethodName));
                xElement.Add(new XAttribute("time",
                                            TimeFormatUtil.FormatMilliseconds(methodTraceInfo.ExecutionTime)));
                foreach (MethodTracer nestedMethodTraceInfo in methodTraceInfo.NestedMethodsTracers)
                {
                    xElement.Add(ConvertMethodTracerToXElement(nestedMethodTraceInfo));
                }

                return xElement;
            }
        }
    }
}
