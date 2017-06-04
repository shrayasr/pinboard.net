using Newtonsoft.Json;
using System;

namespace pinboard.net.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
