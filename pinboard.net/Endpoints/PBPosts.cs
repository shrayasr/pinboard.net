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

        /// <summary>
        /// Returns the most recent time a bookmark was added, updated or deleted.
        /// Use this before calling All to see if the data has changed since the last fetch.
        /// </summary>
        /// <returns>Last updated timestamp</returns>
        public Task<LastUpdate> GetLastUpdate()
        {
            var url = PostsURL
                       .AppendPathSegment("update");

            return MakeRequestAsync<LastUpdate>(url);
        }

        /// <summary>
        /// Returns one or more posts on a single day matching the arguments. If no date or url is given, date of most recent bookmark will be used.
        /// </summary>
        /// <param name="tags">filter by up to three tags</param>
        /// <param name="date">return results bookmarked on this day</param>
        /// <param name="url">return bookmark for this URL</param>
        /// <param name="meta">include a change detection signature in a meta attribute</param>
        /// <returns>Filtered records</returns>
        public Task<GetResult> Get(
            List<string> tags = null, 
            DateTime? date = null, 
            string url = "", 
            bool? meta = null)
       {
            var requestURL = PostsURL
                                .AppendPathSegment("get");

            if (tags != null && tags.Count > 3)
                throw new ArgumentException("Filter can only contain 3 tags at the most.");

            if (tags != null && tags.HasValues())
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

        /// <summary>
        /// Add a bookmark
        /// </summary>
        /// <param name="bookmark">The bookmark to add. URL and Description are mandatory</param>
        /// <returns>Result of the Add operation</returns>
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

        /// <summary>
        /// Update a bookmark
        /// </summary>
        /// <param name="bookmark">Bookmark to update. The key by which the bookmark is updated is the URL. Changing the URL will cause a new bookmark to be added</param>
        /// <param name="updateTime">Update the time also?</param>
        /// <returns>Result of update operation</returns>
        public Task<Result> Update(Bookmark bookmark, bool updateTime = false)
        {
            if (updateTime)
                bookmark.CreatedDate = DateTimeOffset.Now;

            bookmark.Replace = true;

            return Add(bookmark);
        }

        /// <summary>
        /// Delete a bookmark
        /// </summary>
        /// <param name="url">The bookmark identified by this URL is deleted</param>
        /// <returns>Result of the delete operation</returns>
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