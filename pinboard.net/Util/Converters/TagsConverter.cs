using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pinboard.net.Util.Converters
{
    public class TagsConverter : JsonConverter
    {
        private string _delimiter;

        public TagsConverter()
        {
            _delimiter = " ";
        }

        public TagsConverter(string delimiter)
        {
            _delimiter = delimiter;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().Trim();

            if (value.IsEmpty())
                return new List<string>();

            return value
                    .Split(_delimiter.ToCharArray())
                    .ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = (IEnumerable<string>)value;
            var delimitedList = string.Join(_delimiter, list);

            writer.WriteValue(delimitedList);
        }
    }
}
