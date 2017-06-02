using Newtonsoft.Json;
using pinboard.net.Util.Converters;
using System;
using System.Collections.Generic;

namespace pinboard.net.Models
{
    public class Bookmark
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string Extended { get; set; }
        public List<string> Tags { get; set; }

        [JsonProperty("dt")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonConverter(typeof(YesNoConverter))]
        public bool Replace { get; set; }

        [JsonConverter(typeof(YesNoConverter))]
        public bool? Shared { get; set; }

        [JsonConverter(typeof(YesNoConverter))]
        public bool ToRead { get; set; }

        public Bookmark()
        {
            Tags = new List<string>();
            Shared = null;
        }
    }
}