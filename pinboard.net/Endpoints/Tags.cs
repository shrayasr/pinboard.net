using System.Net.Http;

namespace pinboard.net.Endpoints
{
    public class Tags : Endpoint
    {
        public Tags(string apiToken, HttpClient httpClient)
            : base(apiToken, httpClient) { }
    }
}
