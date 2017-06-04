using Newtonsoft.Json;
using pinboard.net.Util.Converters;
using System;
using System.Collections.Generic;

namespace pinboard.net.Models
{
    public class GetResult
    {
        public DateTimeOffset Date { get; set; }
        public string User { get; set; }
        public List<Bookmark> Posts { get; set; }

        public class Bookmark
        {
            public string Href { get; set; }
            public string Description { get; set; }
            public string Extended { get; set; }
            public string Meta { get; set; }
            public string Hash { get; set; }
            public DateTimeOffset Time { get; set; }

            [JsonConverter(typeof(YesNoConverter))]
            public bool Shared { get; set; }

            [JsonConverter(typeof(YesNoConverter))]
            public bool ToRead { get; set; }

            [JsonConverter(typeof(TagsConverter), " ")]
            public List<string> Tags { get; set; }
        }
    }
}
