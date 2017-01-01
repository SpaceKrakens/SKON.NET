#region LICENSE
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

            Console.Write(SKEMA.Write(skemaObj));
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

        [Test]
        public void ReplacingMapElements()
        {
            SKEMAObject obj = new Dictionary<string, SKEMAObject>() { { "Replace", SKEMAObject.Any } };

            Assert.AreEqual(SKEMAType.MAP, obj.Type);
            Assert.AreEqual(SKEMAType.ANY, obj["Replace"].Type);

            obj["Replace"] = SKEMAObject.String;

            Assert.AreEqual(SKEMAType.STRING, obj["Replace"].Type);

            obj["Replace"] = SKEMAObject.ArrayOf(SKEMAObject.Boolean);
            
            Assert.AreEqual(SKEMAType.ARRAY, obj["Replace"].Type);

            Assert.AreEqual(SKEMAType.BOOLEAN, obj["Replace"].ArrayElementSKEMA.Type);

            obj["Replace"].ArrayElementSKEMA = SKEMAObject.ArrayOf(SKEMAObject.Float);
            
            Console.WriteLine(SKEMA.Write(obj));

            Assert.AreEqual(SKEMAType.ARRAY, obj["Replace"].ArrayElementSKEMA.Type);
            
            Assert.AreEqual(SKEMAType.FLOAT, obj["Replace"].ArrayElementSKEMA.ArrayElementSKEMA.Type);
        }

        [Test]
        public void OptionalElements()
        {
            SKEMAObject obj = new Dictionary<string, SKEMAObject>() { { "Required", SKEMAObject.String }, { "Optional", SKEMAObject.Integer } };

            obj.SetOptional("Optional", true);

            Assert.IsTrue(obj.IsOptional("Optional"));

            SKONObject testData = new Dictionary<string, SKONObject>() { { "Required", new SKONObject(SKONObjectTests.TestString) }, { "Optional", new SKONObject(SKONObjectTests.TestInt) } };

            Assert.IsTrue(obj.Valid(testData));
            
            testData.Remove("Optional");

            Assert.IsFalse(testData.ContainsKey("Optional"));

            Assert.IsTrue(obj.Valid(testData));

            testData.Remove("Required");

            Assert.IsFalse(obj.Valid(testData));
        }

        [Test]
        public void ParseOptionalElements()
        {
            string skema =
                @"Required: Any,
                optional Optional: Any,";

            SKEMAObject obj = SKEMA.Parse(skema);

            Assert.IsFalse(obj.IsOptional("Required"));

            Assert.IsTrue(obj.IsOptional("Optional"));
        }
    }
}