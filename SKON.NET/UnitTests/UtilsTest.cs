

namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using SKON.Internal.Utils;

    [TestFixture]
    class UtilsTest
    {
        [Test]
        public void ConvertToValidUnicode()
        {
            string validUnicodeHex = "00D6";

            string result = ParserUtils.ConvertToUnicode(validUnicodeHex);

            Assert.AreEqual("\u00D6", result);

            validUnicodeHex = "FFFE";

            result = ParserUtils.ConvertToUnicode(validUnicodeHex);

            Assert.AreEqual("\uFFFE", result);

            validUnicodeHex = "FEFF";

            result = ParserUtils.ConvertToUnicode(validUnicodeHex);

            Assert.AreEqual("\uFEFF", result);

            validUnicodeHex = string.Empty;

            result = ParserUtils.ConvertToUnicode(validUnicodeHex);

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ConvertToInvalidUnicode()
        {
            string invalidUnicodeHex = "asdf";

            Assert.Throws<FormatException>(() => ParserUtils.ConvertToUnicode(invalidUnicodeHex));

            invalidUnicodeHex = "as";

            Assert.Throws<FormatException>(() => ParserUtils.ConvertToUnicode(invalidUnicodeHex));
        }

        [Test]
        public void EscapeValidUnicode()
        {
            string validUnicodeString = "\\u00D6";

            string result = ParserUtils.EscapeString(validUnicodeString);
            
            Assert.AreEqual("\u00D6", result);

            validUnicodeString = "\\uFEFF\\uFFFE";

            result = ParserUtils.EscapeString(validUnicodeString);

            Assert.AreEqual("\uFEFF\uFFFE", result);

            validUnicodeString = string.Empty;

            result = ParserUtils.EscapeString(validUnicodeString);

            Assert.AreEqual(string.Empty, ParserUtils.EscapeString(validUnicodeString));
        }

        [Test]
        public void EscapeInvalidUnicode()
        {
            string invalidUnicodeString = "\\uasd";

            Assert.Throws<FormatException>(() => ParserUtils.EscapeString(invalidUnicodeString));

            invalidUnicodeString = "\\uGgGg";

            Assert.Throws<FormatException>(() => ParserUtils.EscapeString(invalidUnicodeString));
        }

        [Test]
        public void EscapeValidString()
        {
            string validEscapeString = "\\b\\n\\f\\r\\t\\\"\\\\";

            string result = ParserUtils.EscapeString(validEscapeString);
            
            Assert.AreEqual("\b\n\f\r\t\"\\", result);
        }

        [Test]
        public void EscapeInvalidString()
        {
            string invalidEscapeString = "\\a\\c\\e\t\\\\r'";

            Assert.Throws<FormatException>(() => ParserUtils.EscapeString(invalidEscapeString));

            invalidEscapeString = "\\asdf";

            Assert.Throws<FormatException>(() => ParserUtils.EscapeString(invalidEscapeString));
        }
        
    }
}
