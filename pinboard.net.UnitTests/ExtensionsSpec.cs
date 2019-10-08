using pinboard.net.Util;
using System.Collections.Generic;
using Xunit;

namespace pinboard.net.UnitTests
{
    public class ExtensionsSpec
    {
        [Theory]
        [InlineData(true, null)]
        [InlineData(true, "")]
        [InlineData(true, "        ")]
        [InlineData(false, "string")]
        public void IsEmpty_ReturnsExpectedValue_ForGivenString(bool isEmpty, string input)
        {
            Assert.Equal(isEmpty, input.IsEmpty());
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(false, "")]
        [InlineData(false, "        ")]
        [InlineData(true, "string")]
        public void HasValue_ReturnsExpectedValue_ForGivenString(bool isEmpty, string input)
        {
            Assert.Equal(isEmpty, input.HasValue());
        }

        [Fact]
        public void HasValues_ReturnsTrue_WhenListHasValues()
        {
            var list = new List<string> { "one", "two" };

            Assert.True(list.HasValues());
        }

        [Fact]
        public void HasValues_ReturnsFalse_WhenListIsEmpty()
        {
            var list = new List<string>();

            Assert.False(list.HasValues());
        }

        [Fact]
        public void HasValues_ReturnsFalse_WhenListIsNull()
        {
            List<string> list = null;

            Assert.False(list.HasValues());
        }
    }
}
