using Newtonsoft.Json;
using pinboard.net.Util.Converters;

namespace pinboard.net.Models
{
    public class TagsResult
    {
        [JsonProperty("result")]
        [JsonConverter(typeof(ResultCodeConverter))]
        public bool Result { get; set; }
    }
}
