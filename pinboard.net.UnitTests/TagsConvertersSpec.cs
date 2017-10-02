using System;
using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using pinboard.net.Util.Converters;

namespace pinboard.net.UnitTests
{
    [TestFixture]
    public class TagsConvertersSpec
    {
        [Test]
        [TestCase(typeof(string), true)]
        [TestCase(typeof(int), false)]
        [TestCase(typeof(decimal), false)]
        [TestCase(typeof(DateTime), false)]
        public void ShouldConvertOnlyCorrectType(Type type, bool expected)
        {
            var sut = new TagsConverter();

            var canConvertType = sut.CanConvert(type);

            Assert.That(() => canConvertType, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("", new string[]{}, "")]
        [TestCase("dummyTag dummyTag", new []{"dummyTag", "dummyTag"}, "")]
        [TestCase("dummyTag,dummyTag", new []{"dummyTag", "dummyTag"}, ",")]
        public void ShouldReturnListSplitByDelimeter(string inputTags, string[] expected, string delimeter)
        {
            var jsonReaderMock = new Mock<JsonReader>();
            jsonReaderMock.Setup(m => m.Value).Returns(inputTags);

            var sut = new TagsConverter(delimeter);
            var result = sut.ReadJson(jsonReaderMock.Object, null, null, null) as List<string>;
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        [TestCase(new string[]{}, "", "")]
        [TestCase(new []{"dummyTag", "dummyTag"}, "dummyTag dummyTag", " ")]
        [TestCase(new []{"dummyTag", "dummyTag"}, "dummyTag,dummyTag", ",")]
        public void ShouldWriteCorrectValue(string[] input, string expected, string delimeter)
        {
            var jsonWriterMock = new Mock<JsonWriter>();
            jsonWriterMock.Setup(m => m.WriteValue(It.IsAny<List<string>>()));

            var sut = new TagsConverter(delimeter);
            sut.WriteJson(jsonWriterMock.Object, input, null);

            jsonWriterMock.Verify(m => m.WriteValue(expected));
        }
    }
}