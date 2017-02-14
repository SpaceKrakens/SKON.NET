#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserTests.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using SKON;
    using SKONValueType = SKON.SKONValueType;

    using static UnitTests.SKONObjectTests;

    class ParserTests
    {
        string metadataString = "~Version: 1~\n~DocumentVersion: \"\"~\n";

        [Test]
        public void EmptyInput()
        {
            SKONObject emptyMap = SKON.Parse(metadataString + string.Empty);

            IsNotSimpleType(emptyMap);

            IsNotEmpty(emptyMap);
        }

        [Test]
        public void WhiteSpaceInput()
        {
            SKONObject emptyMap = SKON.Parse(metadataString + " \t\r\n");

            IsNotSimpleType(emptyMap);

            IsNotEmpty(emptyMap);
        }

        [Test]
        public void MultipleCommas()
        {
            string muliCommaSKON = "MultipleCommas: 1,, , ,";

            Assert.Throws<FormatException>(() => SKON.Parse(muliCommaSKON));
        }

        [Test]
        public void ValuelessKeys()
        {
            string valueLessSKON = "FirstKey: ,";

            Assert.Throws<FormatException>(() => SKON.Parse(valueLessSKON));

            valueLessSKON = "FirstKey: , SecondKey: ,";

            Assert.Throws<FormatException>(() => SKON.Parse(valueLessSKON));
        }

        [Test]
        public void KeylessValues()
        {
            string keyLessValuesSKON = "\"Value\",";

            Assert.Throws<FormatException>(() => SKON.Parse(keyLessValuesSKON));

            keyLessValuesSKON = "\"Value\", 1234, 1234.5678,";

            Assert.Throws<FormatException>(() => SKON.Parse(keyLessValuesSKON));
        }

        [Test]
        public void MissingComma()
        {
            string stringSKON = metadataString + "StringKey: \"StringValue\" IntKey: 1234";

            Assert.Throws<FormatException>(() => SKON.Parse(stringSKON));
        }

        [Test]
        public void StringObject()
        {
            string stringSKON = metadataString + "StringKey: \"StringValue\",";

            SKONObject stringMap = SKON.Parse(stringSKON);

            IsNotEmpty(stringMap);

            IsNotSimpleType(stringMap);

            Assert.IsTrue(stringMap.ContainsKey("StringKey"));

            SKONObject stringObj = stringMap["StringKey"];

            HasValue("StringValue", stringObj);
        }
        
        [Test]
        public void IntObject()
        {
            string intSKON = metadataString + "IntKey: 1234,";

            SKONObject intMap = SKON.Parse(intSKON);

            IsNotEmpty(intMap);

            IsNotSimpleType(intMap);

            Assert.IsTrue(intMap.ContainsKey("IntKey"));

            SKONObject intObj = intMap["IntKey"];

            HasValue(1234, intObj);
        }

        [Test]
        public void DoubleObject()
        {
            string doubleSKON = metadataString + "DoubleKey: 1234.5678,";

            SKONObject doubleMap = SKON.Parse(doubleSKON);

            IsNotEmpty(doubleMap);

            IsNotSimpleType(doubleMap);

            Assert.IsTrue(doubleMap.ContainsKey("DoubleKey"));

            SKONObject doubleObj = doubleMap["DoubleKey"];

            HasValue(1234.5678, doubleObj);
        }

        [Test]
        public void BooleanObject()
        {
            string booleanSKON = metadataString + "BooleanKey: true,";

            SKONObject booleanMap = SKON.Parse(booleanSKON);

            IsNotEmpty(booleanMap);

            IsNotSimpleType(booleanMap);

            Assert.IsTrue(booleanMap.ContainsKey("BooleanKey"));

            SKONObject booleanObj = booleanMap["BooleanKey"];

            HasValue(true, booleanObj);
        }

        [Test]
        public void DateTimeObject()
        {
            string dateTimeSKON = metadataString + "DateTimeKey: @1970-01-01,";

            SKONObject dateTimeMap = SKON.Parse(dateTimeSKON);

            IsNotEmpty(dateTimeMap);

            IsNotSimpleType(dateTimeMap);

            Assert.IsTrue(dateTimeMap.ContainsKey("DateTimeKey"));

            SKONObject dateTimeObj = dateTimeMap["DateTimeKey"];

            HasValue(new DateTime(1970, 01, 01), dateTimeObj);
        }

        [Test]
        public void MissingEndBrace()
        {
            string missingBraceSKON = "MissingBraceMap: {";

            Assert.Throws<FormatException>(() => SKON.Parse(missingBraceSKON));

            missingBraceSKON = "MissingBraceMap: { AValue: \"Value\", ";

            Assert.Throws<FormatException>(() => SKON.Parse(missingBraceSKON));
        }

        [Test]
        public void MissingEndBracket()
        {
            string missingBracketSKON = "MissingBracketArray: [";

            Assert.Throws<FormatException>(() => SKON.Parse(missingBracketSKON));

            missingBracketSKON = "MissingBracketArray: [ \"Test\", \"Array\",";

            Assert.Throws<FormatException>(() => SKON.Parse(missingBracketSKON));
        }
        
        [Test]
        public void MissingEndDoubleQuote()
        {
            string missingQuote = "StringKey: \"MissingADoubleQuote,";

            Assert.Throws<FormatException>(() => SKON.Parse(missingQuote));

            missingQuote = "StringKey: \"MisingQuote\\\",";

            Assert.Throws<FormatException>(() => SKON.Parse(missingQuote));
        }

        [Test]
        public void NoSpaces()
        {
            string noSpacesSKON = metadataString + "TestString:\"StringValue\",TestInt:1,TestDouble:1.2,TestBool:true,TestDateTime:@2016-10-09,";

            SKONObject noSpacesMap = SKON.Parse(noSpacesSKON);

            Assert.AreEqual(SKONValueType.MAP, noSpacesMap.Type);

            IsNotEmpty(noSpacesMap);
            IsComplexType(noSpacesMap);
            IsNotSimpleType(noSpacesMap);

            SKONObject stringVal = noSpacesMap["TestString"];

            HasValue("StringValue", stringVal);

            SKONObject intVal = noSpacesMap["TestInt"];

            HasValue(1, intVal);

            SKONObject doubleVal = noSpacesMap["TestDouble"];

            HasValue(1.2, doubleVal);

            SKONObject boolVal = noSpacesMap["TestBool"];

            HasValue(true, boolVal);

            SKONObject dateTimeVal = noSpacesMap["TestDateTime"];

            HasValue(new DateTime(2016, 10, 09), dateTimeVal);
        }
    }
}
