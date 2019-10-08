using pinboard.net.Endpoints;
using System;
using System.Net.Http;

namespace pinboard.net
{
    public class PinboardAPI : IDisposable
    {
        private readonly string _apiToken;
        private HttpClient _httpClient;

        private Posts _posts;
        private Tags _tags;
        private Users _users;
        private Notes _notes;

        public PinboardAPI(string apiToken, HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
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

        public Users Users
        {
            get
            {
                if (_users == null)
                    _users = new Users(_apiToken, _httpClient);

                return _users;
            }
        }

        public Notes Notes
        {
            get
            {
                if (_notes == null)
                    _notes = new Notes(_apiToken, _httpClient);

                return _notes;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
