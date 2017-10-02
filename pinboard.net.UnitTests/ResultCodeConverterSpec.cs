using System;
using NUnit.Framework;
using pinboard.net.Util.Converters;

namespace pinboard.net.UnitTests
{
    [TestFixture]
    public class ResultCodeConverterSpec
    {
        [Test]
        [TestCase(typeof(string), true)]
        [TestCase(typeof(int), false)]
        [TestCase(typeof(decimal), false)]
        [TestCase(typeof(float), false)]
        [TestCase(typeof(DateTime), false)]
        public void ShouldBeAbleToConvertCorrectType(Type type, bool expected)
        {
            var sut = new ResultCodeConverter();
            var canConvertType = sut.CanConvert(type);
            Assert.That(() => canConvertType, Is.EqualTo(expected));
        }
    }
}