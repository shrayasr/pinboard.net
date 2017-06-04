using Flurl;
using Newtonsoft.Json;
using pinboard.net.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace pinboard.net.Endpoints
{
    public class Tags : Endpoint
    {
        public Tags(string apiToken, HttpClient httpClient)
            : base(apiToken, httpClient) { }

        /// <summary>
        /// Returns a full list of the user's tags along with the number of times they were used.
        /// </summary>
        /// <returns>List of tags and their counts</returns>
        public Task<AllTags> Get()
        {
            var url = TagsURL.AppendPathSegment("get");

            return MakeRequestAsync<AllTags>(url, (content) =>
            {
                var allTagCounts = JsonConvert.DeserializeObject<IDictionary<string, int>>(content);

                var allTags = new AllTags();
                foreach(KeyValuePair<string, int> kv in allTagCounts)
                {
                    allTags.Add(new TagCount
                    {
                        Tag = kv.Key,
                        Count = kv.Value
                    });
                }

                return allTags;
            });
        }
    }
}
