using System.IO;
using System.Xml;
using System.Xml.Linq;
using Tracer.Core;

namespace Tracer
{
    namespace Format
    {
        public sealed class XmlTraceResultFormatter : ITraceResultFormatter
        {
            private static readonly XmlWriterSettings XmlWriterSettings = new()
                {OmitXmlDeclaration = true, Indent = true};

            public void Format(Stream outputStream, TraceResult traceResult)
            {
                XDocument xDocument = FormatToXDocument(traceResult);
                using XmlWriter xmlWriter = XmlWriter.Create(outputStream, XmlWriterSettings);
                xDocument.Save(xmlWriter);
            }

            public void Format(TextWriter textWriter, TraceResult traceResult)
            {
                XDocument xDocument = FormatToXDocument(traceResult);
                using XmlWriter xmlWriter = XmlWriter.Create(textWriter, XmlWriterSettings);
                xDocument.Save(xmlWriter);
            }

            private static XDocument FormatToXDocument(TraceResult traceResult)
            {
                var xDocument = new XDocument();
                var rootElement = new XElement(SerializationConfig.XML_ROOT);
                foreach (ThreadTracer threadTracer in traceResult.ThreadTracers.Values)
                {
                    XElement threadElement = ConvertThreadTracerToXElement(threadTracer);
                    rootElement.Add(threadElement);
                }

                xDocument.Add(rootElement);
                return xDocument;
            }

            private static XElement ConvertThreadTracerToXElement(ThreadTracer threadTracer)
            {
                var threadElement = new XElement(SerializationConfig.XML_THREAD);
                threadElement.Add(new XAttribute(SerializationConfig.XML_THREAD_ID, threadTracer.ThreadId));
                threadElement.Add(new XAttribute(SerializationConfig.XML_THREAD_TIME,
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
                var xElement = new XElement(SerializationConfig.XML_METHOD);
                xElement.Add(new XAttribute(SerializationConfig.XML_METHOD_CLASS, methodTraceInfo.TypeName));
                xElement.Add(new XAttribute(SerializationConfig.XML_METHOD_NAME, methodTraceInfo.MethodName));
                xElement.Add(new XAttribute(SerializationConfig.XML_METHOD_TIME,
                                            TimeFormatUtil.FormatMilliseconds(methodTraceInfo.ExecutionTime)));
                foreach (MethodTracer nestedMethodTraceInfo in methodTraceInfo.NestedMethodTracers)
                    xElement.Add(ConvertMethodTracerToXElement(nestedMethodTraceInfo));

                return xElement;
            }
        }
    }
}
