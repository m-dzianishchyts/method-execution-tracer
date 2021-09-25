using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tracer
{
    namespace Format
    {
        public class JsonTimeMsConverter : JsonConverter<long>
        {
            private const string ON_NULL_DEFAULT_TIME = "0";

            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string formattedTime = reader.GetString() ?? ON_NULL_DEFAULT_TIME;
                long numericTime = TimeFormatUtil.ExtractNumericTime(formattedTime);
                return numericTime;
            }

            public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(TimeFormatUtil.FormatMilliseconds(value));
            }
        }
    }
}
