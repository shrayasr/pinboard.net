using Newtonsoft.Json;
using System;

namespace pinboard.net.Models
{
    public class LastPostUpdateDetails
    {
        [JsonProperty("update_time")]
        public DateTimeOffset UpdateTime { get; set; }
    }
}