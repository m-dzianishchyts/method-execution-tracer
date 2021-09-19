using System;

namespace Tracer.Core
{
    public class TraceResult
    {
        public string TypeName { get; }
        public string MethodName { get; }
        public long ExecutionTime { get; }

        public TraceResult(string typeName, string methodName, long executionTime)
        {
            TypeName = typeName;
            MethodName = methodName;
            ExecutionTime = executionTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            return obj.GetType() == GetType() && Equals((TraceResult) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeName, MethodName, ExecutionTime);
        }

        public override string ToString()
        {
            return $"{nameof(TypeName)}: {TypeName}, " +
                   $"{nameof(MethodName)}: {MethodName}, " +
                   $"{nameof(ExecutionTime)}: {ExecutionTime}ms";
        }

        private bool Equals(TraceResult other)
        {
            return TypeName == other.TypeName && MethodName == other.MethodName &&
                   ExecutionTime == other.ExecutionTime;
        }
    }
}