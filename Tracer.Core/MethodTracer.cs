using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer.Core
{
    public sealed class MethodTracer
    {
        private readonly Stopwatch _stopwatch;
        private readonly IList<MethodTracer> _nestedMethodsTracers;
        public string TypeName { get; }
        public string MethodName { get; }
        public long ExecutionTime => _stopwatch.ElapsedMilliseconds;

        public IEnumerable<MethodTracer> NestedMethodsTracers => _nestedMethodsTracers;

        internal MethodTracer(MethodBase method)
        {
            MethodName = method.Name;
            Type declaringType = method.DeclaringType ??
                                 throw new ArgumentException($"{nameof(method)} has no declaring type.");
            TypeName = declaringType.Name;
            _stopwatch = new Stopwatch();
            _nestedMethodsTracers = new List<MethodTracer>();
        }

        internal void StartTrace()
        {
            _stopwatch.Restart();
        }

        internal void StopTrace()
        {
            _stopwatch.Stop();
        }

        internal void AddNestedMethod(MethodTracer methodTracer)
        {
            _nestedMethodsTracers.Add(methodTracer);
        }
    }
}