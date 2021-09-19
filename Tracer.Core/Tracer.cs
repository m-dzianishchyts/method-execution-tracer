using System;
using System.Diagnostics;

namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private long _executionTime;
        private string _typeName;
        private string _methodName;
        private readonly Stopwatch _stopwatch;

        public Tracer()
        {
            _executionTime = 0;
            _methodName = "";
            _typeName = "";
            _stopwatch = new Stopwatch();
        }

        public void StartTrace()
        {
            var callingMethod = new StackFrame(1).GetMethod();
            if (callingMethod == null)
            {
                throw new TracerException("Not enough stack frames to find calling method.");
            }

            _typeName = callingMethod.GetType().Name;
            _methodName = callingMethod.Name;
            _stopwatch.Start();
        }

        public void StopTrace()
        {
            if (!_stopwatch.IsRunning)
            {
                throw new TracerException($"{nameof(StartTrace)} method has been not called yet. " +
                                          "Stopwatch is not running.");
            }

            var callingMethod = new StackFrame(1).GetMethod();
            if (callingMethod == null)
            {
                throw new TracerException("Not enough stack frames to find calling method.");
            }

            var methodName = callingMethod.Name;
            var typeName = callingMethod.GetType().Name;
            if (!Equals(methodName, _methodName) || !Equals(typeName, _typeName))
            {
                throw new TracerException("Tracer must be stopped in the method " +
                                          $"in which it was started: {_typeName}:{_methodName}().");
            }

            _stopwatch.Stop();
            try
            {
                _executionTime = Convert.ToInt64(_stopwatch.Elapsed.TotalMilliseconds);
            }
            catch (OverflowException)
            {
                _executionTime = long.MaxValue;
            }
            finally
            {
                _stopwatch.Reset();
            }
        }

        public TraceResult GetTraceResult()
        {
            if (_typeName == null || _methodName == null)
            {
                throw new TracerException("Tracer has been not started yet.");
            }

            var traceResult = new TraceResult(_typeName, _methodName, _executionTime);
            return traceResult;
        }
    }
}