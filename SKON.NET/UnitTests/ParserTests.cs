namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using SKON;

    using static UnitTests.SKONObjectTest;

    class ParserTests
    {
        [Test]
        public void Test_EmptyInput()
        {
            SKONObject emptyMap = SKON.Parse(string.Empty);

            Test_NotSimpleType(emptyMap);

            Test_IsNotEmpty(emptyMap);
        }

        [Test]
        public void Test_WhiteSpaceInput()
        {
            SKONObject emptyMap = SKON.Parse(" \t\r\n");

            Test_NotSimpleType(emptyMap);

            //Test_IsNotEmpty(emptyMap);
        }

        [Test]
        public void Test_StringObject()
        {
            string stringSKON = "StringKey: \"StringValue\",";

            SKONObject stringMap = SKON.Parse(stringSKON);

            Test_IsNotEmpty(stringMap);

            Test_NotSimpleType(stringMap);

            Assert.IsTrue(stringMap.ContainsKey("StringKey"));

            SKONObject stringObj = stringMap["StringKey"];

            TestObject("StringValue", stringObj);
        }
        
        [Test]
        public void Test_StringObject_MissingComma()
        {
            string stringSKON = "StringKey: \"StringValue\"";
            
            Assert.Throws<FormatException>(() => SKON.Parse(stringSKON));
        }

        [Test]
        public void Test_IntObject()
        {
            string intSKON = "IntKey: 1234,";

            SKONObject intMap = SKON.Parse(intSKON);

            Test_IsNotEmpty(intMap);

            Test_NotSimpleType(intMap);

            Assert.IsTrue(intMap.ContainsKey("IntKey"));

            SKONObject intObj = intMap["IntKey"];

            TestObject(1234, intObj);
        }

    }
}
