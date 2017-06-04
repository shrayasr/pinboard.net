using Flurl;
using Newtonsoft.Json;
using pinboard.net.Models;
using pinboard.net.Util;
using System;
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

        /// <summary>
        /// Delete an existing tag.
        /// </summary>
        /// <param name="tag">The tag to delete</param>
        /// <returns>A result code</returns>
        public Task<TagsResult> Delete(string tag)
        {
            if (tag.IsEmpty())
                throw new ArgumentException("Tag cannot be empty");

            var url = TagsURL
                        .AppendPathSegment("delete")
                        .SetQueryParam("tag", tag);

            return MakeRequestAsync<TagsResult>(url);
        }

        /// <summary>
        /// Rename an tag, or fold it in to an existing tag
        /// </summary>
        /// <param name="from">source tag. note: match is not case sensitive</param>
        /// <param name="to">destination tag</param>
        /// <returns>A result code</returns>
        public Task<TagsResult> Rename(string from, string to)
        {
            if (from.IsEmpty() || to.IsEmpty())
                throw new ArgumentException("from/to cannot be empty");

            var url = TagsURL
                        .AppendPathSegment("rename")
                        .SetQueryParam("old", from)
                        .SetQueryParam("new", to);

            return MakeRequestAsync<TagsResult>(url);
        }
    }
}
