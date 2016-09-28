using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SKON;

using static UnitTests.SKONObjectTest;

namespace UnitTests
{
    class ParserTests
    {
        
        [Test]
        public void Test_StringObject()
        {
            string stringSKON = "StringKey: \"StringValue\",";

            SKONObject stringMap = SKON.SKON.Parse(stringSKON);

            Test_IsNotEmpty(stringMap);

            Test_NotSimpleType(stringMap);

            Assert.IsTrue(stringMap.ContainsKey("StringKey"));

            SKONObject stringObj = stringMap["StringKey"];

            TestObject("StringValue", stringObj);
        }
        
    }
}
