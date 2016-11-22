﻿#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SKEMATests.cs" company="SpaceKrakens">
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
    using SKON.SKEMA;
    using SKON;
    
    [TestFixture]
    class SKEMAObjectTests
    {
        [Test]
        public void AnySKEMA()
        {
            Assert.IsTrue(SKEMAObject.Any.Valid(SKONObjectTests.TestMap));
        }

        [Test]
        public void WrongTypeSKEMA()
        {
            Assert.IsFalse(SKEMAObject.String.Valid(SKONObjectTests.TestInt));
        }

        [Test]
        public void StringSKEMA()
        {
            Assert.IsTrue(SKEMAObject.String.Valid(SKONObjectTests.TestString));
        }

        [Test]
        public void IntegerSKEMA()
        {
            Assert.IsTrue(SKEMAObject.Integer.Valid(SKONObjectTests.TestInt));
        }

        [Test]
        public void DoubleSKEMA()
        {
            Assert.IsTrue(SKEMAObject.Float.Valid(SKONObjectTests.TestDouble));
        }

        [Test]
        public void BooleanSKEMA()
        {
            Assert.IsTrue(SKEMAObject.Boolean.Valid(SKONObjectTests.TestBoolean));
        }

        [Test]
        public void DateTimeSKEMA()
        {
            Assert.IsTrue(SKEMAObject.DateTime.Valid(SKONObjectTests.TestDateTime));
        }

        [Test]
        public void ArraySKEMA()
        {
            SKEMAObject obj = SKEMAObject.ArrayOf(SKEMAObject.Float);

            Assert.IsFalse(obj.Valid(new int[] { 1, 2, 3 }));

            Assert.IsTrue(obj.Valid(new double[] { 1, 2, 3 }));

            obj = SKEMAObject.ArrayOf(SKEMAObject.Any);

            Assert.IsTrue(obj.Valid(new int[] { 1, 2, 3 }));

            Assert.IsTrue(obj.Valid(new double[] { 1, 2, 3 }));
        }
        
        [Test]
        public void MapSKEMA()
        {
            SKEMAObject map = new SKEMAObject(new Dictionary<string, SKEMAObject>() { { "StringKey", SKEMAObject.String }, { "Any", SKEMAObject.Any } });

            SKONObject data = new Dictionary<string, SKONObject>() { { "StringKey", "Value" }, { "Any", 1.3d } };

            Assert.IsTrue(map.Valid(data));

            data["Any"] = new DateTime(1990, 06, 02);

            Assert.IsTrue(map.Valid(data));

            data = new Dictionary<string, SKONObject>() { { "StringKey", 123 }, { "Any", "AnyValue" } };

            Assert.IsFalse(map.Valid(data));

            data["StringKey"] = "asdf";

            Assert.IsTrue(map.Valid(data));
        }

        [Test]
        public void CyclicReferences()
        {
            Assert.Throws<FormatException>(() => SKEMA.Parse(
                @"define One: #Two,
                define Two: #One,

                Value: [ #One ],"));
        }

        [Test]
        public void TestSKEMA()
        {
            SKEMAObject testSKEMA = global::SKON.SKEMA.TestSKEMA.TestSKEMAObject;

            SKONObject testData = TestSKON.TestSKONObject;

            Assert.IsTrue(testSKEMA.Valid(testData));
        }

        [Test]
        public void GenericParsing()
        {
            string skema = 
                @"define Color: { Red: Integer, Blue: Integer, Green: Integer, },

                Colors: [ #Color ],";

            SKEMAObject skemaObj = SKEMA.Parse(skema);
        }

        [Test]
        public void TreeSKEMA()
        {
            string skema = 
                @"define Node: 
                { 
                    Value: Any,
                    optional Nodes: [ #Node ],
                },
            
                Tree: #Node,";

            SKEMAObject skemaObj = SKEMA.Parse(skema);
        }
    }
}