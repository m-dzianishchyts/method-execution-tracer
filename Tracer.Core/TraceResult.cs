using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tracer
{
    namespace Core
    {
        public sealed class TraceResult
        {
            internal TraceResult(IDictionary<int, ThreadTracer> threadsTraceResults)
            {
                ThreadTracers = threadsTraceResults.ToImmutableDictionary();
            }

            public ImmutableDictionary<int, ThreadTracer> ThreadTracers { get; }
        }
    }
}
