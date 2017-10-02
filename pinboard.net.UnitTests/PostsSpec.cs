using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;
using pinboard.net.Endpoints;

namespace pinboard.net.UnitTests
{
    [TestFixture]
    public class PostsSpec
    {
        [Test]
        public void GetShouldThrowWhenCalledWithMoreThan3Tags()
        {
            var sut = new Posts("", new HttpClient());
            var tags = new List<string> {"dummyTag", "dummyTag", "dummyTag", "dummyTag"};
            Assert.Throws<ArgumentException>(() => sut.Get(tags));
        }

        [Test]
        public void GetShouldThrowWithInformativeMessage()
        {
            var sut = new Posts("", new HttpClient());
            var tags = new List<string> {"dummyTag", "dummyTag", "dummyTag", "dummyTag"};
            Assert.That(() => sut.Get(tags),
                Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Filter can only contain 3 tags at the most."));
        }
    }
}