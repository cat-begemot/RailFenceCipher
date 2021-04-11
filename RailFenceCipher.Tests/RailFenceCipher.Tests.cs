using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace RailFenceCipher.Tests
{
    public class Tests
    {
	    private const int MinAllowedRailCount = 2;
	    private readonly string TestMessage = "This is original test message.";
	    
	    [TestCase("WEAREDISCOVEREDFLEEATONCE", 3, "WECRLTEERDSOEEFEAOCAIVDEN")]
	    [TestCase("123456789", 2, "135792468")]
	    [TestCase("123456789", 3, "159246837")]
	    [TestCase("123456789", 4, "172683594")]
	    [TestCase("1", 2, "1")]
	    [TestCase("1", 3, "1")]
        public void EncodeMessage_Successful(string originalMessage, int railCount, string encodedMessage)
        {
	        var result = RailFenceCipher.Encode(originalMessage, railCount);

            Assert.AreEqual(encodedMessage, result);
        }

        [Test]
        public void EncodeMessage_EmptyString_ReturnEmptyString()
        {
	        var result = RailFenceCipher.Encode(string.Empty, MinAllowedRailCount);

	        Assert.AreEqual(string.Empty, result);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void EncodeMessage_WrongRailCount_ThrowException(int railCount)
        {
	        Assert.Throws<ArgumentOutOfRangeException>(() => RailFenceCipher.Encode(TestMessage, railCount));
        }

        [TestCase("WECRLTEERDSOEEFEAOCAIVDEN", 3, "WEAREDISCOVEREDFLEEATONCE")]
        [TestCase("135792468", 2, "123456789")]
        [TestCase("159246837", 3, "123456789")]
        [TestCase("172683594", 4, "123456789")]
        [TestCase("1", 2, "1")]
        [TestCase("1", 3, "1")]
        public void DecodeMessage_Successful(string encodedMessage, int railCount, string originalMessage)
        {
	        var result = RailFenceCipher.Decode(encodedMessage, railCount);

	        Assert.AreEqual(originalMessage, result);
        }

        [Test]
        public void DecodeMessage_EmptyString_ReturnEmptyString()
        {
	        var result = RailFenceCipher.Decode(string.Empty, MinAllowedRailCount);

	        Assert.AreEqual(string.Empty, result);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void DecodeMessage_WrongRailCount_ThrowException(int railCount)
        {
	        Assert.Throws<ArgumentOutOfRangeException>(() => RailFenceCipher.Decode(TestMessage, railCount));
        }

        [Test]
        public void EncodeDecode_Successful()
        {
	        var result = RailFenceCipher.Decode(
		        RailFenceCipher.Encode(TestMessage, MinAllowedRailCount), MinAllowedRailCount);

            Assert.AreEqual(TestMessage, result);
        }
    }
}