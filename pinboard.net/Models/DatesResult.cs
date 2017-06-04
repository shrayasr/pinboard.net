using Newtonsoft.Json;
using pinboard.net.Util.Converters;
using System.Collections.Generic;

namespace pinboard.net.Models
{
    public class DatesResult
    {
        public string User { get; set; }

        [JsonConverter(typeof(TagsConverter), "+")]
        public List<string> Tag { get; set; }

        public IDictionary<string, int> Dates { get; set; }
    }
}
