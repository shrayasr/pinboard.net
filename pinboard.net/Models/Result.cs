using Newtonsoft.Json;
using pinboard.net.Util.Converters;

namespace pinboard.net.Models
{
    public class Result
    {
        [JsonProperty("result_code")]
        [JsonConverter(typeof(ResultCodeConverter))]
        public bool ResultCode { get; set; }
    }
}