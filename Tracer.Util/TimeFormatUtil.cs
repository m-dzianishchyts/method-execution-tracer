using System.Linq;

namespace Tracer
{
    namespace Format
    {
        public static class TimeFormatUtil
        {
            private const string UNIT_MILLISECONDS = "ms";
            private const string ON_NULL_DEFAULT_NUMERIC_TIME = "0";

            public static string FormatMilliseconds(long milliseconds)
            {
                return $"{milliseconds}{UNIT_MILLISECONDS}";
            }

            public static long ExtractNumericTime(string formattedTime)
            {
                var numericTime = formattedTime.Where(char.IsDigit).ToString();
                return long.Parse(numericTime ?? ON_NULL_DEFAULT_NUMERIC_TIME);
            }
        }
    }
}
