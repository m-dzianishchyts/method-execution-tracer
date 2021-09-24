using System.Linq;

namespace Tracer
{
    namespace Util
    {
        public static class TimeFormatUtil
        {
            public static string FormatMilliseconds(long milliseconds)
            {
                return $"{milliseconds}ms";
            }

            public static long ExtractNumericTime(string formattedTime)
            {
                var numericTime = formattedTime.Where(char.IsDigit).ToString();
                return long.Parse(numericTime ?? "0");
            }
        }
    }
}