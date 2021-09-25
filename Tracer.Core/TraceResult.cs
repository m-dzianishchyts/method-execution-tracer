using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tracer
{
    namespace Core
    {
        public sealed class TraceResult
        {
            public ImmutableDictionary<string, ThreadTracer> ThreadTracers { get; }

            internal TraceResult(IDictionary<string, ThreadTracer> threadsTraceResults)
            {
                ThreadTracers = threadsTraceResults.ToImmutableDictionary();
            }
        }
    }
}
