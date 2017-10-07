using pinboard.net.Endpoints;
using System;
using System.Collections.Generic;
using System.Net.Http;
using pinboard.net.Models;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class PostsSpec
    {
        [Fact]
        public void GetShouldThrowWhenCalledWithMoreThan3Tags()
        {
            var sut = new Posts("", new HttpClient());
            var tags = new List<string> {"dummyTag", "dummyTag", "dummyTag", "dummyTag"};
            Assert.ThrowsAsync<ArgumentException>(() => sut.Get(tags));
        }

        [Fact]
        public void GetShouldThrowWithInformativeMessage()
        {
            var expectedMessage = "Filter can only contain 3 tags at the most.";
            var sut = new Posts("", new HttpClient());
            var tags = new List<string> {"dummyTag", "dummyTag", "dummyTag", "dummyTag"};
            var exception = Record.ExceptionAsync(() => sut.Get(tags)).Result;
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void AddShouldThrowWhenUrlIsEmptyWithInformativeMessage()
        {
            var sut = new Posts("", new HttpClient());
            var dummyBookmark = new Bookmark();
            var exception = Record.ExceptionAsync(() => sut.Add(dummyBookmark)).Result;
            Assert.Equal("Bookmark's URL cannot be empty", exception.Message);
        }

        [Fact]
        public void AddShouldThrowWhenDesciptionIsEmptyWithInformativeMessage()
        {
            var sut = new Posts("", new HttpClient());
            var dummyBookmark = new Bookmark
            {
                Url = "dummy"
            };
            var exception = Record.ExceptionAsync(() => sut.Add(dummyBookmark)).Result;
            Assert.Equal("Bookmark's Description cannot be empty", exception.Message);
        }
    }
}