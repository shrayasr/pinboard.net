using System;
using Newtonsoft.Json;

namespace pinboard.net.Util.Converters
{
    public class ResultCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool returnValue = (reader.Value.ToString() == "done");
            return returnValue;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not implemented because CanWrite is set to false");
        }
    }
}