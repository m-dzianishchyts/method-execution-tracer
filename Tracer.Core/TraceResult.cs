using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tracer.Core
{
    public sealed class TraceResult
    {
        public ImmutableDictionary<int, ThreadTracer> ThreadsTraceResults { get; }

        internal TraceResult(IDictionary<int, ThreadTracer> threadsTraceResults)
        {
            ThreadsTraceResults = threadsTraceResults.ToImmutableDictionary();
        }
    }
}
