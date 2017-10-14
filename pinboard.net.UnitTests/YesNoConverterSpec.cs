using System;
using Moq;
using Newtonsoft.Json;
using pinboard.net.Util.Converters;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class YesNoConverterSpec
    {
        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(decimal), false)]
        [InlineData(typeof(DateTime), false)]
        public void ShouldConvertOnlyCorrectType(Type type, bool expected)
        {
            var sut = new YesNoConverter();
            var canConvert = sut.CanConvert(type);
            Assert.Equal(expected, canConvert);
        }

        [Theory]
        [InlineData(null, "no")]
        [InlineData(false, "no")]
        [InlineData(true, "yes")]
        public void ShouldWriteCorrectValue(object value, string expected)
        {
            var jsonWriterMock = new Mock<JsonWriter>();
            jsonWriterMock.Setup(m => m.WriteValue(It.IsAny<string>()));
            var sut = new YesNoConverter();

            sut.WriteJson(jsonWriterMock.Object, value, null);

            jsonWriterMock.Verify(m => m.WriteValue(expected));
        }
    }
}