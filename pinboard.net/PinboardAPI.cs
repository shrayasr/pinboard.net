using pinboard.net.Endpoints;
using System;
using System.Net.Http;

namespace pinboard.net
{
    public class PinboardAPI : IDisposable
    {
        private HttpClient _httpClient;
        private string _apiToken;

        private Posts _posts;
        private Tags _tags;

        public PinboardAPI(string apiToken)
        {
            _httpClient = new HttpClient();
            _apiToken = apiToken;
        }

        public Posts Posts
        {
            get
            {
                if (_posts == null)
                    _posts = new Posts(_apiToken, _httpClient);

                return _posts;
            }
        }

        public Tags Tags
        {
            get
            {
                if (_tags == null)
                    _tags = new Tags(_apiToken, _httpClient);

                return _tags;
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
