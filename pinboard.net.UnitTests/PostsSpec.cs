using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using pinboard.net.Endpoints;
using pinboard.net.Models;
using RichardSzalay.MockHttp;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class PostsSpec : IDisposable
    {
        readonly MockHttpMessageHandler _mockHandler;
        readonly HttpClient _httpClient;
        readonly Posts _postRequest;

        public PostsSpec()
        {
            _mockHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHandler, true);
            _postRequest = new Posts("token", _httpClient);
        }

        [Fact]
        public void GetLastUpdate_ReturnsDateAsExpected()
        {
            var expectedUpdateTime = "2011-03-24T19:02:07Z";
            _mockHandler.Expect(GetFullPostsUri("/posts/update"))
                .WithExactQueryString(BaseQueryString)
                .Respond("application/json", $"{{'update_time' : '{expectedUpdateTime}'}}");

            var lastPostUpdateDetails = _postRequest.GetLastUpdate().Result;

            Assert.Equal(DateTime.Parse(expectedUpdateTime), lastPostUpdateDetails.UpdateTime);
        }

        [Fact]
        public void GetShouldThrowWhenCalledWithMoreThan3Tags()
        {
            var sut = new Posts("", _httpClient);
            var tags = new List<string> {"dummy", "dummy", "dummy", "dummy" };
            Assert.ThrowsAsync<ArgumentException>(() => _postRequest.Get(tags));
        }

        [Fact]
        public void GetShouldThrowWithInformativeMessage()
        {
            var expectedMessage = "Filter can only contain 3 tags at the most.";
            var tags = new List<string> { "dummy", "dummy", "dummy", "dummy" };
            var exception = Record.ExceptionAsync(() => _postRequest.Get(tags)).Result;
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void GetLastUpdate_PassesTagsAsExpected()
        {
            var expectedTags = new List<string> { "this", "that", "other" };
            _mockHandler.Expect(GetFullPostsUri("/posts/get"))
                .WithExactQueryString($"tag={Uri.EscapeDataString(string.Join(",", expectedTags))}&meta=&{BaseQueryString}")
                .Respond("application/json", $"{{'posts' : [{{'tags':'{string.Join(" ", expectedTags)}'}}]}}");

            var dayWisePosts = _postRequest.Get(expectedTags).Result;

            Assert.False(dayWisePosts.Posts[0].Tags.Except(expectedTags).Any());
        }

        [Theory]
        [InlineData(null, "no")]
        [InlineData(false, "no")]
        [InlineData(true, "yes")]
        public void GetLastUpdate_ReturnsExpectedSharedValue(bool expectedSharedValue, string jsonSharedValue)
        {
            _mockHandler.Expect(GetFullPostsUri("/posts/get"))
                .WithExactQueryString($"meta=&{BaseQueryString}")
                .Respond("application/json", $"{{'posts' : [{{'shared':'{jsonSharedValue}'}}]}}");

            var dayWisePosts = _postRequest.Get().Result;

            Assert.Equal(expectedSharedValue, dayWisePosts.Posts[0].Shared);
        }

        [Fact]
        public void AddShouldThrowWhenUrlIsEmptyWithInformativeMessage()
        {
            var sut = new Posts("", _httpClient);
            var dummyBookmark = new Bookmark();
            var exception = Record.ExceptionAsync(() => sut.Add(dummyBookmark)).Result;
            Assert.Equal("Bookmark's URL cannot be empty", exception.Message);
        }

        [Fact]
        public void AddShouldThrowWhenDesciptionIsEmptyWithInformativeMessage()
        {
            var sut = new Posts("", _httpClient);
            var dummyBookmark = new Bookmark
            {
                Url = "dummy"
            };
            var exception = Record.ExceptionAsync(() => sut.Add(dummyBookmark)).Result;
            Assert.Equal("Bookmark's Description cannot be empty", exception.Message);
        }

        private string GetFullPostsUri(string path) => $"https://api.pinboard.in/v1{path}";

        private string BaseQueryString => "auth_token=token&format=json";

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}