using Flurl;
using pinboard.net.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace pinboard.net.Endpoints
{
    public class Users : Endpoint
    {
        public Users(string apiToken, HttpClient httpClient)
            :base(apiToken, httpClient)
        { }

        /// <summary>
        /// Returns the user's secret RSS key (for viewing private feeds)
        /// </summary>
        /// <returns>Secret RSS Key</returns>
        public Task<SecretRSSKey> Secret()
        {
            var url = UsersURL.AppendPathSegment("secret");
            return MakeRequestAsync<SecretRSSKey>(url);
        }

        /// <summary>
        /// Returns the user's API token (for making API calls without a password)
        /// </summary>
        /// <returns>API Token</returns>
        public Task<ApiToken> ApiToken()
        {
            var url = UsersURL.AppendPathSegment("api_token");
            return MakeRequestAsync<ApiToken>(url);
        }
    }
}
