using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;

namespace pinboard.net.Endpoints
{
    public class PBEndpoint
    {
        private string _apiToken;
        private HttpClient _httpClient;

        private const string BASEURL = "https://api.pinboard.in/v1";

        public const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ssK";
        public const string DATE_FORMAT = "yyyy-MM-dd";

        protected string PostsURL
        {
            get { return BASEURL + "/posts"; }
        }

        protected string TagsURL
        {
            get { return BASEURL + "/tags"; }
        }

        protected string UsersURL
        {
            get { return BASEURL + "/user"; }
        }

        protected string NotesURL
        {
            get { return BASEURL + "/notes"; }
        }

        public PBEndpoint(string apiToken, HttpClient httpClient)
        {
            _apiToken = apiToken;
            _httpClient = httpClient;
        }

        protected async Task<T> MakeRequestAsync<T>(Url url)
        {
            url
             .SetQueryParam("auth_token", _apiToken)
             .SetQueryParam("format", "json");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        protected string GetYesNo(bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? "yes" : "no";
            }
            else
            {
                return "";
            }
        }
    }
}