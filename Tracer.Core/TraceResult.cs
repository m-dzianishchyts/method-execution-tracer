using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tracer
{
    namespace Core
    {
        public sealed class TraceResult
        {
            public ImmutableDictionary<int, ThreadTracer> ThreadTracers { get; }

            internal TraceResult(IDictionary<int, ThreadTracer> threadsTraceResults)
            {
                ThreadTracers = threadsTraceResults.ToImmutableDictionary();
            }
        }
    }
}
