using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tracer
{
    namespace Core
    {
        public sealed class TraceResult
        {
            public ImmutableDictionary<string, ThreadTracer> ThreadsTraceResults { get; }

            internal TraceResult(IDictionary<string, ThreadTracer> threadsTraceResults)
            {
                ThreadsTraceResults = threadsTraceResults.ToImmutableDictionary();
            }
        }
    }
}
