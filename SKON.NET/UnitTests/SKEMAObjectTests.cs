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
            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(
                @"define One: #Two,
                define Two: #One,

                Value: [ #One ],"));

            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(
                @"define One: #One,"));

            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(
                @"define One: 
                {
                    Two: #Two,
                    optional One: #One,
                    Three: #Three,
                },
                
                define Two: { Three: [ #Three ], },

                define Three: [ #One ],

                One: #One,"));

            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(
                @"define One: #Two,
                define Two: #Three,
                define Three: #Four,
                define Four: #Five,
                define Five: #Six,
                define Six: #Seven,
                define Seven: #Eight,
                define Eight: #Nine,
                define Nine: #Ten,
                define Ten: #One,"));

            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(
                @"define One: #Two,
                define Two: #One,

                define A: #B,
                define B: #A,

                A: #One,"));
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
                @"define Color: { Red: Integer, Green: Integer, Blue: Integer, },

                Colors: [ #Color ],";

            SKEMAObject skemaObj = SKEMA.Parse(skema);

            Console.Write(SKEMA.Write(skemaObj));

            SKONObject data = new Dictionary<string, SKONObject>
            {
                {
                    "Colors", new SKONObject[] 
                    {
                        new Dictionary<string, SKONObject>
                        {
                            { "Red", 140 },
                            { "Green", 50 },
                            { "Blue", 235 },
                        },

                        new Dictionary<string, SKONObject>
                        {
                            { "Red", 50 },
                            { "Green", 50 },
                            { "Blue", 50 },
                        },
                    }
                },
            };

            Assert.IsTrue(skemaObj.Valid(data));

            data["Colors"].Add(new Dictionary<string, SKONObject> { { "H", 260 }, { "S", 0.3 }, { "V", 1 } });

            Assert.IsFalse(skemaObj.Valid(data));
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

            SKONObject data = new Dictionary<string, SKONObject>
            {
                {
                    "Tree", new Dictionary<string, SKONObject>
                    {
                        {
                            "Value", "TestString"
                        },

                        {
                            "Nodes", new SKONObject[]
                            {
                                new Dictionary<string, SKONObject>
                                {
                                    { "Value", 10 },
                                    {
                                        "Nodes", new SKONObject[]
                                        {
                                            new Dictionary<string, SKONObject>
                                            {
                                                { "Value", true }
                                            }
                                        }
                                    }
                                },

                                new Dictionary<string, SKONObject>
                                {
                                    { "Value", DateTime.Now }
                                }
                            }
                        }
                    }
                }
            };

            Assert.IsTrue(skemaObj.Valid(data));
        }

        [Test]
        public void SelfReferencingSKEMA()
        {
            string skema = @"define A: #A, A: #A,";

            Assert.Throws<CouldNotSolveReferencesException>(() => SKEMA.Parse(skema));
            
            skema = @"define A: { optional A: #A, }, A: #A,";

            SKEMAObject obj = SKEMA.Parse(skema);

            string res = SKEMA.Write(obj);

            Console.WriteLine(res);

            SKEMAObject resObj = SKEMA.Parse(res);

            // FIXME! The SKEMAObject.Equals method can requrse. This leads to a stack overflow!
            // To fix this multiple methods could be used.
            // The best way is probably to do a similar solution to the FlattenTree method in SKEMA and keep a stack of all the compared nodes

            //Assert.IsTrue(resObj == obj);
            
            skema = @"define A: { optional A: #A, optional B: #B, }, define B: #A, A: #A, ";

            obj = SKEMA.Parse(skema);

            res = SKEMA.Write(obj);

            Console.WriteLine(res);

            resObj = SKEMA.Parse(res);

            Assert.IsTrue(resObj == obj);
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

        [Test]
        public void WriteSKEMA()
        {
            string skema =
               @"define Color: { Red: Integer, Green: Integer, Blue: Integer, },

                Colors: [ #Color ],";

            SKEMAObject skemaObj = SKEMA.Parse(skema);

            string res = SKEMA.Write(skemaObj);

            SKEMAObject newSkemaObj = SKEMA.Parse(res);

            Assert.AreEqual(skemaObj, newSkemaObj);

            Console.WriteLine(res);

            skema =
                @"define Node: 
                { 
                    Value: Any,
                    optional Nodes: [ #Node ],
                },
            
                Tree: #Node,";

            skemaObj = SKEMA.Parse(skema);

            res = SKEMA.Write(skemaObj);

            newSkemaObj = SKEMA.Parse(res);

            //Assert.AreEqual(skemaObj, newSkemaObj);

            Console.WriteLine(res);
        }

        [Test]
        public void WriteRecursiveSKEMA()
        {
            SKEMAObject skema1 = new Dictionary<string, SKEMAObject>();

            SKEMAObject skema2 = new Dictionary<string, SKEMAObject> { { "Test", skema1 } };

            skema1.Add("Test2", skema2);
            
            //string res = SKEMA.Write(skema1);
        }

        [Test]
        public void ObjectEquality()
        {
            SKEMAObject obj1 = SKEMAObject.String;

            SKEMAObject obj2 = SKEMAObject.String;

            Assert.IsTrue(obj1.Equals(obj2));

            Assert.IsTrue(obj1 == obj2);

            Assert.IsTrue(obj1.GetHashCode() == obj2.GetHashCode());

            obj1 = SKEMAObject.AsReference(obj1, "Test");

            obj2 = SKEMAObject.AsReference(obj2, "Test");

            Assert.IsTrue(obj1.Equals(obj2));

            Assert.IsTrue(obj1 == obj2);

            Assert.IsTrue(obj1.GetHashCode() == obj2.GetHashCode());

            obj2 = SKEMAObject.Any;

            Assert.IsFalse(obj1.Equals(obj2));

            Assert.IsFalse(obj1 == obj2);

            obj1 = new Dictionary<string, SKEMAObject> { { "Test", SKEMAObject.Float } };

            Assert.IsFalse(obj1.Equals(obj2));

            Assert.IsFalse(obj1 == obj2);
        }
    }
}