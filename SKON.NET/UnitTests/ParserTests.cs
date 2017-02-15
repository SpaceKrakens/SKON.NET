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
        const string metadataString = "-Version: 1-\n-DocumentVersion: \"\"-\n";

        private static SKONObject ParseWithMetadata(string skon)
        {
            Console.WriteLine(metadataString + skon);
            SKONMetadata meta;
            return ParseWithMetadata(skon, out meta);
        }

        private static SKONObject ParseWithMetadata(string skon, out SKONMetadata meta)
        {
            return SKON.Parse(metadataString + skon, out meta);
        }

        [Test]
        public void EmptyInput()
        {
            Assert.Throws<FormatException>(() => SKON.Parse(string.Empty));

            SKONObject emptyMap = ParseWithMetadata(string.Empty);

            IsNotSimpleType(emptyMap);

            IsNotEmpty(emptyMap);
        }

        [Test]
        public void WhiteSpaceInput()
        {
            SKONObject emptyMap = ParseWithMetadata(" \t\r\n");

            IsNotSimpleType(emptyMap);

            IsNotEmpty(emptyMap);
        }

        [Test]
        public void MultipleCommas()
        {
            string muliCommaSKON = "MultipleCommas: 1,, , ,";

            Assert.Throws<FormatException>(() => ParseWithMetadata(muliCommaSKON));
        }

        [Test]
        public void ValuelessKeys()
        {
            string valueLessSKON = "FirstKey: ,";

            Assert.Throws<FormatException>(() => ParseWithMetadata(valueLessSKON));

            valueLessSKON = "FirstKey: , SecondKey: ,";

            Assert.Throws<FormatException>(() => ParseWithMetadata(valueLessSKON));
        }

        [Test]
        public void KeylessValues()
        {
            string keyLessValuesSKON = "\"Value\",";

            Assert.Throws<FormatException>(() => ParseWithMetadata(keyLessValuesSKON));

            keyLessValuesSKON = "\"Value\", 1234, 1234.5678,";

            Assert.Throws<FormatException>(() => ParseWithMetadata(keyLessValuesSKON));
        }

        [Test]
        public void MissingComma()
        {
            string stringSKON = "StringKey: \"StringValue\" IntKey: 1234";

            Assert.Throws<FormatException>(() => ParseWithMetadata(stringSKON));
        }

        [Test]
        public void StringObject()
        {
            string stringSKON = "StringKey: \"StringValue\",";

            SKONObject stringMap = ParseWithMetadata(stringSKON);

            IsNotEmpty(stringMap);

            IsNotSimpleType(stringMap);

            Assert.IsTrue(stringMap.ContainsKey("StringKey"));

            SKONObject stringObj = stringMap["StringKey"];

            HasValue("StringValue", stringObj);
        }
        
        [Test]
        public void IntObject()
        {
            string intSKON = "IntKey: 1234,";

            SKONObject intMap = ParseWithMetadata(intSKON);

            IsNotEmpty(intMap);

            IsNotSimpleType(intMap);

            Assert.IsTrue(intMap.ContainsKey("IntKey"));

            SKONObject intObj = intMap["IntKey"];

            HasValue(1234, intObj);
        }

        [Test]
        public void FloatObject()
        {
            string floatSKON = "FloatKey: 1234.5678,";

            Console.WriteLine(floatSKON);

            SKONObject doubleMap = ParseWithMetadata(floatSKON);

            IsNotEmpty(doubleMap);

            IsNotSimpleType(doubleMap);

            Assert.IsTrue(doubleMap.ContainsKey("FloatKey"));

            SKONObject doubleObj = doubleMap["FloatKey"];

            HasValue(1234.5678, doubleObj);
        }

        [Test]
        public void BooleanObject()
        {
            string booleanSKON = "BooleanKey: true,";
            
            SKONObject booleanMap = ParseWithMetadata(booleanSKON);

            Console.WriteLine(SKON.Write(booleanMap));

            IsNotEmpty(booleanMap);

            IsNotSimpleType(booleanMap);

            Assert.IsTrue(booleanMap.ContainsKey("BooleanKey"));

            SKONObject booleanObj = booleanMap["BooleanKey"];

            HasValue(true, booleanObj);
        }

        [Test]
        public void DateTimeObject()
        {
            string dateTimeSKON = "DateTimeKey: 1970-01-01,";

            SKONObject dateTimeMap = ParseWithMetadata(dateTimeSKON);

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

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingBraceSKON));

            missingBraceSKON = "MissingBraceMap: { AValue: \"Value\", ";

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingBraceSKON));
        }

        [Test]
        public void MissingEndBracket()
        {
            string missingBracketSKON = "MissingBracketArray: [";

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingBracketSKON));

            missingBracketSKON = "MissingBracketArray: [ \"Test\", \"Array\",";

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingBracketSKON));
        }
        
        [Test]
        public void MissingEndDoubleQuote()
        {
            string missingQuote = "StringKey: \"MissingADoubleQuote,";

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingQuote));

            missingQuote = "StringKey: \"MisingQuote\\\",";

            Assert.Throws<FormatException>(() => ParseWithMetadata(missingQuote));
        }

        [Test]
        public void NoSpaces()
        {
            string noSpacesSKON = "TestString:\"StringValue\",TestInt:1,TestDouble:1.2,TestBool:true,TestDateTime:2016-10-09,";

            SKONObject noSpacesMap = ParseWithMetadata(noSpacesSKON);

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

        [Test]
        public void HardParsing()
        {
            string skon = "DifficultTokens: \"_[{]}:;,\", _: 1,__:[\"]\",],";

            SKONObject skonObj = ParseWithMetadata(skon);

            Assert.IsTrue(skonObj.ContainsKey("DifficultTokens"));
            Assert.IsTrue(skonObj.ContainsKey("_"));
            Assert.IsTrue(skonObj.ContainsKey("__"));

            Assert.AreEqual("_[{]}:;,", skonObj["DifficultTokens"].String);
            Assert.AreEqual(1, skonObj["_"].Int);
            Assert.AreEqual("]", skonObj["__"][0].String);
        }

        [Test]
        public void ParseWriteParse()
        {
            string skon = @"Boolean: true,
                            Int: 12,
                            Map: { Content: ""Hello"", },";

            SKONMetadata meta;
            SKONObject obj = ParseWithMetadata(skon, out meta);

            string res = SKON.Write(obj, meta);

            SKONMetadata meta2;
            SKONObject objRes = SKON.Parse(res, out meta2);

            HasKey(obj, "Boolean", SKONValueType.BOOLEAN);
            HasKey(obj, "Int", SKONValueType.INTEGER);
            HasKey(obj, "Map", SKONValueType.MAP);
            HasKey(obj["Map"], "Content", SKONValueType.STRING);

            Assert.IsTrue(meta.LanguageVersion == meta2.LanguageVersion);
            Assert.IsTrue(meta.DocuemntVersion == meta2.DocuemntVersion);
            Assert.IsTrue(meta.SKEMA == meta2.SKEMA);
            Assert.IsTrue(obj == objRes);
        }
    }
}
