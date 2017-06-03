using Newtonsoft.Json;
using System;

namespace pinboard.net.Models
{
    public class LastUpdate
    {
        [JsonProperty("update_time")]
        public DateTimeOffset UpdateTime { get; set; }
    }
}