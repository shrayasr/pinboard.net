using System;
using pinboard.net.Util.Converters;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class ResultCodeConverterSpec
    {
        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(decimal), false)]
        [InlineData(typeof(float), false)]
        [InlineData(typeof(DateTime), false)]
        public void ShouldBeAbleToConvertCorrectType(Type type, bool expected)
        {
            var sut = new ResultCodeConverter();
            var canConvertType = sut.CanConvert(type);
            Assert.Equal(expected, canConvertType);
        }
    }
}