using System;
using Newtonsoft.Json;

namespace pinboard.net.Util.Converters
{
    public class YesNoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (reader.Value.ToString() == "yes");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (bool?)value;

            if (val.HasValue && val.Value)
                writer.WriteValue("yes");
            else
                writer.WriteValue("no");
        }
    }
}