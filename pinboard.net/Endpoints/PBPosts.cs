using Flurl;
using pinboard.net.Models;
using pinboard.net.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace pinboard.net.Endpoints
{
    public class PBPosts : PBEndpoint
    {
        public PBPosts(string apiToken, HttpClient httpClient)
            : base(apiToken, httpClient) { }

        public Task<LastUpdate> GetLastUpdate()
        {
            var url = PostsURL
                       .AppendPathSegment("update");

            return MakeRequestAsync<LastUpdate>(url);
        }

        public Task<GetResult> Get(
            List<string> tags = null, 
            DateTime? date = null, 
            string url = "", 
            bool? meta = null)
        {
            if (tags.Count > 3)
                throw new ArgumentException("Filter can only contain 3 tags at the most.");

            var requestURL = PostsURL
                                .AppendPathSegment("get");

            if (tags.HasValues())
            {
                var tagsString = string.Join(",", tags);
                requestURL.SetQueryParam("tag", tagsString);
            }

            if (date.HasValue && date.Value != DateTimeOffset.MinValue)
            {
                requestURL.SetQueryParam("dt", date.Value.ToString(DATE_FORMAT));
            }

            if (url.HasValue())
            {
                requestURL.SetQueryParam("url", url);
            }

            requestURL.SetQueryParam("meta", GetYesNo(meta));

            return MakeRequestAsync<GetResult>(requestURL);
        }

        public Task<Result> Add(Bookmark bookmark)
        {
            var url = PostsURL
                        .AppendPathSegment("add");

            if (bookmark.Url.IsEmpty())
                throw new ArgumentException("Bookmark's URL cannot be empty");

            if (bookmark.Description.IsEmpty())
                throw new ArgumentException("Bookmark's Description cannot be empty");

            url
                .SetQueryParam("url", bookmark.Url)
                .SetQueryParam("description", bookmark.Description);

            if (bookmark.Extended.HasValue())
                url.SetQueryParam("extended", bookmark.Extended);

            if (bookmark.Tags.Count > 100)
                throw new ArgumentException("A bookmark can't have > 100 tags");

            var commaSeparatedTags = string.Join(",", bookmark.Tags);
            url.SetQueryParam("tags", commaSeparatedTags);

            if (bookmark.CreatedDate != DateTimeOffset.MinValue)
                url.SetQueryParam("dt", bookmark.CreatedDate.ToString(DATETIME_FORMAT));

            url.SetQueryParam("replace", GetYesNo(bookmark.Replace));
            url.SetQueryParam("shared", GetYesNo(bookmark.Shared));
            url.SetQueryParam("toread", GetYesNo(bookmark.ToRead));

            return MakeRequestAsync<Result>(url);
        }

        public Task<Result> Update(Bookmark bookmark, bool updateTime = false)
        {
            if (updateTime)
                bookmark.CreatedDate = DateTimeOffset.Now;

            bookmark.Replace = true;

            return Add(bookmark);
        }

        public Task<Result> Delete(string url)
        {
            var requestURL = PostsURL
                                .AppendPathSegment("delete");

            if (url.IsEmpty())
                throw new ArgumentException("URL is a mandatory field to delete a bookmark");

            requestURL.SetQueryParam("url", url);

            return MakeRequestAsync<Result>(requestURL);
        }
    }
}