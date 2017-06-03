using pinboard.net.Endpoints;
using System;
using System.Net.Http;

namespace pinboard.net
{
    public class PinboardAPI : IDisposable
    {
        private HttpClient _httpClient;
        private string _apiToken;

        private PBPosts _posts;

        public PinboardAPI(string apiToken)
        {
            _httpClient = new HttpClient();
            _apiToken = apiToken;
        }

        public PBPosts Posts
        {
            get
            {
                if (_posts == null)
                    _posts = new PBPosts(_apiToken, _httpClient);

                return _posts;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            _httpClient.Dispose();
        }
        #endregion

    }
}
