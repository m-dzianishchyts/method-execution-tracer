using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tracer
{
    namespace Util
    {
        public class JsonTimeMsConverter : JsonConverter<long>
        {
            public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string formattedTime = reader.GetString() ?? "0";
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
