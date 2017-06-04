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
        /// Returns a list of dates with the number of posts at each date.
        /// </summary>
        /// <param name="tags">filter by up to three tags</param>
        /// <returns>List of dates, filtered</returns>
        public Task<DatesResult> Dates(List<string> tags = null)
        {
            var url = PostsURL
                                .AppendPathSegment("dates");

            if (tags != null && tags.Count > 3)
                throw new ArgumentException("Filter can only contain 3 tags at the most.");

            if (tags != null && tags.HasValues())
            {
                var tagsString = string.Join(",", tags);
                url.SetQueryParam("tag", tagsString);
            }

            return MakeRequestAsync<DatesResult>(url);
        }

        /// <summary>
        /// Returns all bookmarks in the user's account.
        /// </summary>
        /// <param name="tags">filter by up to three tags</param>
        /// <param name="start">offset value (default is 0)</param>
        /// <param name="results">number of results to return. Default is all</param>
        /// <param name="fromDate">return only bookmarks created after this time</param>
        /// <param name="toDate">return only bookmarks created before this time</param>
        /// <param name="meta">include a change detection signature for each bookmark</param>
        /// <returns>Filtered list of bookmarks</returns>
        public Task<AllResult> All(
            List<string> tags = null,
            int start = 0,
            int? results = null,
            DateTimeOffset? fromDate = null,
            DateTimeOffset? toDate = null,
            int? meta = null)
        {
            var url = PostsURL
                        .AppendPathSegment("all");

            if (tags != null)
            {
                if (tags.Count > 3)
                    throw new ArgumentException("Filter can only contain 3 tags at the most.");

                if (tags.HasValues())
                {
                    var tagsString = string.Join(",", tags);
                    url.SetQueryParam("tag", tagsString);
                }
            }

            url.SetQueryParam("start", start);

            if (results.HasValue)
                url.SetQueryParam("results", results.Value);

            if (fromDate.HasValue)
                url.SetQueryParam("fromdt", fromDate.Value.ToString(DATETIME_FORMAT));

            if (toDate.HasValue)
                url.SetQueryParam("todt", toDate.Value.ToString(DATETIME_FORMAT));

            if (meta.HasValue)
                url.SetQueryParam("meta", meta.Value);

            return MakeRequestAsync<AllResult>(url);
        }

        /// <summary>
        /// Returns a list of the user's most recent posts, filtered by tag.
        /// </summary>
        /// <param name="tags">filter by up to three tags</param>
        /// <param name="count">number of results to return. Default is 15, max is 100</param>
        /// <returns>Filtered list of bookmarks</returns>
        public Task<RecentResult> Recent(List<string> tags = null, int count = 15)
        {
            var requestURL = PostsURL
                                .AppendPathSegment("recent");

            if (tags != null && tags.Count > 3)
                throw new ArgumentException("Filter can only contain 3 tags at the most.");

            if (tags != null && tags.HasValues())
            {
                var tagsString = string.Join(",", tags);
                requestURL.SetQueryParam("tag", tagsString);
            }

            if (count > 100)
                throw new ArgumentException("Max Count is 100");

            requestURL.SetQueryParam("count", count);

            return MakeRequestAsync<RecentResult>(requestURL);
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