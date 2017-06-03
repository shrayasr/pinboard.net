using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pinboard.net.Util.Converters
{
    class TagsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader
                    .Value
                    .ToString()
                    .Split(' ')
                    .ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = (List<string>)value;
            var commaSepList = string.Join(" ", list);

            writer.WriteValue(commaSepList);
        }
    }
}
