using System;
using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;
using pinboard.net.Util.Converters;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class TagsConvertersSpec
    {
        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(decimal), false)]
        [InlineData(typeof(DateTime), false)]
        public void ShouldConvertOnlyCorrectType(Type type, bool expected)
        {
            var sut = new TagsConverter();

            var canConvertType = sut.CanConvert(type);

            Assert.Equal(expected, canConvertType);
        }

        [Theory]
        [InlineData(new string[] { }, "", "")]
        [InlineData(new[] {"dummyTag", "dummyTag"}, "dummyTag dummyTag", " ")]
        [InlineData(new[] {"dummyTag", "dummyTag"}, "dummyTag,dummyTag", ",")]
        public void ShouldReturnListSplitByDelimeter(string[] expected, string inputTags, string delimeter)
        {
            var jsonReaderMock = new Mock<JsonReader>();
            jsonReaderMock.Setup(m => m.Value).Returns(inputTags);

            var sut = new TagsConverter(delimeter);
            var result = sut.ReadJson(jsonReaderMock.Object, null, null, null) as List<string>;
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new string[] { }, "", "")]
        [InlineData(new[] { "dummyTag", "dummyTag" }, "dummyTag dummyTag", " ")]
        [InlineData(new[] { "dummyTag", "dummyTag" }, "dummyTag,dummyTag", ",")]
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