#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SKONObjectTests.cs" company="SpaceKrakens">
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

    [TestFixture]
    class SKONObjectTests
    {
        public static string TestString = "This is a test string!";
        public static int TestInt = 1234;
        public static double TestDouble = 1234.5678d;
        public static bool TestBoolean = true;
        public static DateTime TestDateTime = new DateTime(1970, 01, 01);

        public static string TestKey = "TestKey";

        public static SKONObject TestMap => new SKONObject(new Dictionary<string, SKONObject>()
            {
                { "Empty", new SKONObject() },
                { "String", new SKONObject(TestString) },
                { "Int", new SKONObject(TestInt) },
                { "Double", new SKONObject(TestDouble) },
                { "Bool", new SKONObject(TestBoolean) },
                { "DateTime", new SKONObject(TestDateTime) }
            });

        public static SKONObject TestStringList => new SKONObject(new SKONObject[] { new SKONObject(), new SKONObject(TestString), new SKONObject(TestInt), new SKONObject(TestDouble), new SKONObject(TestBoolean), new SKONObject(TestDateTime) });

        public static void IsComplexType(SKONObject obj)
        {
            Assert.IsTrue(SKONValueType.MAP == obj.Type || SKONValueType.ARRAY == obj.Type);

            if (obj.Type == SKONValueType.ARRAY)
            {
                Assert.GreaterOrEqual(obj.Length, 0);
                Assert.AreEqual(obj.Values.Count, obj.Length);
            }
            
            Assert.GreaterOrEqual(obj.Keys.Count, 0);
        }

        public static void IsNotComplexType(SKONObject obj)
        {
            Assert.AreNotEqual(SKONValueType.MAP, obj.Type);
            Assert.AreNotEqual(SKONValueType.ARRAY, obj.Type);

            Assert.AreEqual(-1, obj.Length);
            Assert.AreEqual(0, obj.Values.Count);
            Assert.AreEqual(0, obj.Keys.Count);

            Assert.IsTrue(obj[TestKey].IsEmpty);
            Assert.IsTrue(obj[0].IsEmpty);
        }

        public static void IsSimpleType(SKONObject obj)
        {
            bool simpleType = false;

            simpleType |= obj.Type == SKONValueType.STRING;
            simpleType |= obj.Type == SKONValueType.INTEGER;
            simpleType |= obj.Type == SKONValueType.FLOAT;
            simpleType |= obj.Type == SKONValueType.BOOLEAN;
            simpleType |= obj.Type == SKONValueType.DATETIME;

            Assert.IsTrue(simpleType);

            bool hasSimpleValue = false;

            hasSimpleValue |= obj.String != null;
            hasSimpleValue |= obj.Int != null;
            hasSimpleValue |= obj.Double != null;
            hasSimpleValue |= obj.Boolean != null;
            hasSimpleValue |= obj.DateTime != null;

            Assert.IsTrue(hasSimpleValue);
        }

        public static void IsNotSimpleType(SKONObject obj)
        {
            Assert.AreNotEqual(SKONValueType.STRING, obj.Type);
            Assert.AreNotEqual(SKONValueType.INTEGER, obj.Type);
            Assert.AreNotEqual(SKONValueType.FLOAT, obj.Type);
            Assert.AreNotEqual(SKONValueType.BOOLEAN, obj.Type);
            Assert.AreNotEqual(SKONValueType.DATETIME, obj.Type);
            
            Assert.IsNull(obj.String);
            Assert.IsNull(obj.Int);
            Assert.IsNull(obj.Double);
            Assert.IsNull(obj.Boolean);
            Assert.IsNull(obj.DateTime);
        }

        public static void IsEmpty(SKONObject emptyObj)
        {
            Assert.AreEqual(SKONValueType.EMPTY, emptyObj.Type, "Empty SKONObject.Type is not EMPTY!");

            Assert.IsTrue(emptyObj.IsEmpty, "Empty SKONObject.IsEmpty returned false!");

            Assert.IsNull(emptyObj.String, "Empty SKONObject returned non-null string!");
            Assert.IsNull(emptyObj.Int, "Empty SKONObject returned non-null int!");
            Assert.IsNull(emptyObj.Double, "Empty SKONObject returned non-null double!");
            Assert.IsNull(emptyObj.Boolean, "Empty SKONObject returned non-null boolean!");
            Assert.IsNull(emptyObj.DateTime, "Empty SKONObject returned non-null DateTime!");

            Assert.AreEqual(-1, emptyObj.Length);
            Assert.AreEqual(0, emptyObj.Values.Count);
            Assert.AreEqual(0, emptyObj.Keys.Count);

            // Should this kind of thing be tested for different keys and indexes?
            Assert.IsTrue(emptyObj[TestKey].IsEmpty);
            Assert.IsTrue(emptyObj[0].IsEmpty);
        }

        public static void IsNotEmpty(SKONObject obj)
        {
            Assert.IsFalse(obj.IsEmpty);
            
            bool hasValue = false;

            hasValue |= obj.String != null;
            hasValue |= obj.Int != null;
            hasValue |= obj.Double != null;
            hasValue |= obj.Boolean != null;
            hasValue |= obj.DateTime != null;
            
            hasValue |= obj.Length >= 0;
            // These have no meaning
            hasValue |= obj.Keys.Count >= 0;
            hasValue |= obj.Values.Count >= 0;
            
            Assert.IsTrue(hasValue, "SKONObject does not contain any value!");
        }

        public static void HasValue(string expected, SKONObject actual)
        {
            Assert.AreEqual(SKONValueType.STRING, actual.Type);

            IsNotEmpty(actual);

            IsSimpleType(actual);

            IsNotComplexType(actual);

            Assert.AreEqual(expected, actual.String);
        }

        public static void HasValue(int expected, SKONObject actual)
        {
            Assert.AreEqual(SKONValueType.INTEGER, actual.Type);

            IsNotEmpty(actual);

            IsSimpleType(actual);

            IsNotComplexType(actual);

            Assert.AreEqual(expected, actual.Int);
        }

        public static void HasValue(double expected, SKONObject actual)
        {
            Assert.AreEqual(SKONValueType.FLOAT, actual.Type);

            IsNotEmpty(actual);

            IsSimpleType(actual);

            IsNotComplexType(actual);

            Assert.AreEqual(expected, actual.Double);
        }

        public static void HasValue(bool expected, SKONObject actual)
        {
            Assert.AreEqual(SKONValueType.BOOLEAN, actual.Type);

            IsNotEmpty(actual);

            IsSimpleType(actual);

            IsNotComplexType(actual);

            Assert.AreEqual(expected, actual.Boolean);
        }

        public static void HasValue(DateTime expected, SKONObject actual)
        {
            Assert.AreEqual(SKONValueType.DATETIME, actual.Type);

            IsNotEmpty(actual);

            IsSimpleType(actual);

            IsNotComplexType(actual);
            
            Assert.AreEqual(expected, actual.DateTime);
        }
        
        public static void HasKey(SKONObject obj, string key, SKONValueType type)
        {
            Assert.AreEqual(SKONValueType.MAP, obj.Type);
            Assert.IsTrue(obj.ContainsKey(key));
            Assert.AreEqual(type, obj[key].Type);
        }

        [Test]
        public void EmptyValue()
        {
            IsEmpty(new SKONObject());
        }
        
        [Test]
        public void StringValue()
        {
            SKONObject stringObj = new SKONObject(TestString);

            Assert.AreEqual(SKONValueType.STRING, stringObj.Type);

            IsNotEmpty(stringObj);

            Assert.IsNull(stringObj.Int);
            Assert.IsNull(stringObj.Double);
            Assert.IsNull(stringObj.Boolean);
            Assert.IsNull(stringObj.DateTime);

            Assert.AreEqual(TestString, stringObj.String);

            IsSimpleType(stringObj);

            IsNotComplexType(stringObj);
        }

        [Test]
        public void IntValue()
        {
            SKONObject intObj = new SKONObject(TestInt);

            Assert.AreEqual(SKONValueType.INTEGER, intObj.Type);

            IsNotEmpty(intObj);

            Assert.IsNull(intObj.String);
            Assert.IsNull(intObj.Double);
            Assert.IsNull(intObj.Boolean);
            Assert.IsNull(intObj.DateTime);
            
            Assert.AreEqual(TestInt, intObj.Int);

            IsSimpleType(intObj);

            IsNotComplexType(intObj);
        }

        [Test]
        public void DoubleValue()
        {
            SKONObject doubleObj = new SKONObject(TestDouble);

            Assert.AreEqual(SKONValueType.FLOAT, doubleObj.Type);

            IsNotEmpty(doubleObj);

            Assert.IsNull(doubleObj.String);
            Assert.IsNull(doubleObj.Int);
            Assert.IsNull(doubleObj.Boolean);
            Assert.IsNull(doubleObj.DateTime);

            Assert.AreEqual(TestDouble, doubleObj.Double);

            IsSimpleType(doubleObj);

            IsNotComplexType(doubleObj);
        }

        [Test]
        public void BooleanValue()
        {
            SKONObject booleanObj = new SKONObject(TestBoolean);

            Assert.AreEqual(SKONValueType.BOOLEAN, booleanObj.Type);

            IsNotEmpty(booleanObj);

            Assert.IsNull(booleanObj.String);
            Assert.IsNull(booleanObj.Int);
            Assert.IsNull(booleanObj.Double);
            Assert.IsNull(booleanObj.DateTime);

            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            IsSimpleType(booleanObj);

            IsNotComplexType(booleanObj);
        }

        [Test]
        public void DateTimeValue()
        {
            SKONObject dateTimeObj = new SKONObject(TestDateTime);

            Assert.AreEqual(SKONValueType.DATETIME, dateTimeObj.Type);

            IsNotEmpty(dateTimeObj);

            Assert.IsNull(dateTimeObj.String);
            Assert.IsNull(dateTimeObj.Int);
            Assert.IsNull(dateTimeObj.Double);
            Assert.IsNull(dateTimeObj.Boolean);

            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);

            IsSimpleType(dateTimeObj);

            IsNotComplexType(dateTimeObj);
        }

        [Test]
        public void MapValue()
        {
            SKONObject mapObj = TestMap;

            Assert.AreEqual(SKONValueType.MAP, mapObj.Type);

            IsNotEmpty(mapObj);

            IsComplexType(mapObj);

            IsNotSimpleType(mapObj);
            
            Assert.IsTrue(mapObj[TestKey].IsEmpty);
            
            Assert.AreEqual(6, mapObj.Keys.Count);
            
            Assert.IsTrue(mapObj.ContainsAllKeys("Empty", "String", "Int", "Double", "Bool", "DateTime"));
            Assert.IsFalse(mapObj.ContainsAllKeys(TestKey));
            
            SKONObject emptyObj = mapObj["Empty"];
            IsEmpty(emptyObj);

            SKONObject stringObj = mapObj["String"];
            IsNotEmpty(stringObj);
            Assert.AreEqual(TestString, stringObj.String);

            SKONObject intObj = mapObj["Int"];
            IsNotEmpty(intObj);
            Assert.AreEqual(TestInt, intObj.Int);

            SKONObject doubleObj = mapObj["Double"];
            IsNotEmpty(doubleObj);
            Assert.AreEqual(TestDouble, doubleObj.Double);

            SKONObject booleanObj = mapObj["Bool"];
            IsNotEmpty(booleanObj);
            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            SKONObject dateTimeObj = mapObj["DateTime"];
            IsNotEmpty(dateTimeObj);
            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);
        }

        [Test]
        public void ListValue()
        {
            SKONObject listObj = TestStringList;

            Assert.AreEqual(SKONValueType.ARRAY, listObj.Type);
            
            IsNotEmpty(listObj);

            IsComplexType(listObj);

            IsNotSimpleType(listObj);

            Assert.AreNotEqual(-1, listObj.Length);

            Assert.AreEqual(6, listObj.Values.Count);

            Assert.AreEqual(listObj.Values.Count, listObj.Length);

            SKONObject emptyObj = listObj[0];
            IsEmpty(emptyObj);
            IsNotComplexType(emptyObj);
            IsNotSimpleType(emptyObj);

            SKONObject stringObj = listObj[1];
            IsNotEmpty(stringObj);
            IsNotComplexType(stringObj);
            Assert.AreEqual(TestString, stringObj.String);

            SKONObject intObj = listObj[2];
            IsNotEmpty(intObj);
            IsNotComplexType(intObj);
            Assert.AreEqual(TestInt, intObj.Int);

            SKONObject doubleObj = listObj[3];
            IsNotEmpty(doubleObj);
            IsNotComplexType(doubleObj);
            Assert.AreEqual(TestDouble, doubleObj.Double);

            SKONObject booleanObj = listObj[4];
            IsNotEmpty(booleanObj);
            IsNotComplexType(booleanObj);
            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            SKONObject dateTimeObj = listObj[5];
            IsNotEmpty(dateTimeObj);
            IsNotComplexType(dateTimeObj);
            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);
        }

        [Test]
        public void ObjectEquality()
        {
            Assert.AreEqual(new SKONObject(TestString), new SKONObject(TestString));

            Assert.AreEqual(new SKONObject(TestInt), new SKONObject(TestInt));

            Assert.AreEqual(new SKONObject(TestDouble), new SKONObject(TestDouble));

            Assert.AreEqual(new SKONObject(TestBoolean), new SKONObject(TestBoolean));

            Assert.AreEqual(new SKONObject(TestDateTime), new SKONObject(TestDateTime));
        }
        
        [Test]
        public void HashEquality()
        {
            Assert.AreEqual(new SKONObject(TestString).GetHashCode(), new SKONObject(TestString).GetHashCode());

            Assert.AreEqual(new SKONObject(TestInt).GetHashCode(), new SKONObject(TestInt).GetHashCode());

            Assert.AreEqual(new SKONObject(TestDouble).GetHashCode(), new SKONObject(TestDouble).GetHashCode());

            Assert.AreEqual(new SKONObject(TestBoolean).GetHashCode(), new SKONObject(TestBoolean).GetHashCode());

            Assert.AreEqual(new SKONObject(TestDateTime).GetHashCode(), new SKONObject(TestDateTime).GetHashCode());
        }

        [Test]
        public void ObjectInequality()
        {
            Assert.AreNotEqual(new SKONObject(), new SKONObject(TestString));

            Assert.AreNotEqual(new SKONObject(), new SKONObject(TestInt));

            Assert.AreNotEqual(new SKONObject(), new SKONObject(TestDouble));

            Assert.AreNotEqual(new SKONObject(), new SKONObject(TestBoolean));

            Assert.AreNotEqual(new SKONObject(), new SKONObject(TestDateTime));

            
            Assert.AreNotEqual(new SKONObject(TestString), new SKONObject(TestInt));

            Assert.AreNotEqual(new SKONObject(TestString), new SKONObject(TestDouble));

            Assert.AreNotEqual(new SKONObject(TestString), new SKONObject(TestBoolean));

            Assert.AreNotEqual(new SKONObject(TestString), new SKONObject(TestDateTime));

            
            Assert.AreNotEqual(new SKONObject(TestInt), new SKONObject(TestDouble));

            Assert.AreNotEqual(new SKONObject(TestInt), new SKONObject(TestBoolean));

            Assert.AreNotEqual(new SKONObject(TestInt), new SKONObject(TestDateTime));

            
            Assert.AreNotEqual(new SKONObject(TestDouble), new SKONObject(TestBoolean));

            Assert.AreNotEqual(new SKONObject(TestDouble), new SKONObject(TestDateTime));
            

            Assert.AreNotEqual(new SKONObject(TestBoolean), new SKONObject(TestDateTime));
        }

        [Test]
        public void ObjectEqualityOperator()
        {
            SKONObject obj1 = new SKONObject(TestString);
            SKONObject obj2 = new SKONObject(TestString);

            Assert.IsTrue(obj1 == obj2);

            Assert.IsTrue(obj1 == TestString);
            Assert.IsTrue(obj2 == TestString);

            Assert.IsTrue(obj1.String == TestString);
            Assert.IsTrue(obj2.String == TestString);

            float f = 0.4f;

            Assert.IsFalse(obj1 == f);
            Assert.IsFalse(obj2 == f);
        }

        [Test]
        public void ConstructorEquality()
        {
            SKONObject obj1 = new SKONObject(TestString);
            SKONObject obj2 = TestString;

            Assert.AreEqual(obj1, obj2);

            obj1 = new SKONObject(TestInt);
            obj2 = TestInt;

            Assert.AreEqual(obj1, obj2);

            obj1 = new SKONObject(TestDouble);
            obj2 = TestDouble;

            Assert.AreEqual(obj1, obj2);

            obj1 = new SKONObject(TestBoolean);
            obj2 = TestBoolean;

            Assert.AreEqual(obj1, obj2);

            obj1 = new SKONObject(TestDateTime);
            obj2 = TestDateTime;

            Assert.AreEqual(obj1, obj2);

        }

        [Test]
        public void MapGet()
        {
            SKONObject map = TestMap;
            
            string s = map.Get("String", "Wrong");

            Assert.AreEqual(TestString, s);

            int i = map.Get("Int", int.MinValue);

            Assert.AreEqual(TestInt, i);

            double d = map.Get("Double", double.MinValue);

            Assert.AreEqual(TestDouble, d);
            
            bool b = map.Get("Bool", !TestBoolean);

            Assert.AreEqual(TestBoolean, b);

            DateTime dt = map.Get("DateTime", DateTime.MinValue);

            Assert.AreEqual(TestDateTime, dt);
        }
        
        [Test]
        public void MapTryGet()
        {
            SKONObject map = TestMap;

            string s;

            Assert.IsFalse(map.TryGet(TestString, out s));

            Assert.AreEqual(default(string), s);

            Assert.IsTrue(map.TryGet("String", out s));

            Assert.AreEqual(TestString, s);

            int i;

            Assert.IsTrue(map.TryGet("Int", out i));

            Assert.AreEqual(TestInt, i);

            double d;

            Assert.IsTrue(map.TryGet("Double", out d));

            Assert.AreEqual(TestDouble, d);

            bool b;

            Assert.IsTrue(map.TryGet("Bool", out b));

            Assert.AreEqual(TestBoolean, b);

            DateTime dt;

            Assert.IsTrue(map.TryGet("DateTime", out dt));

            Assert.AreEqual(TestDateTime, dt);
        }

        [Test]
        public void ArrayModification()
        {
            SKONObject arrayObj = new string[] { "This", "is", "an", "array" };

            IsNotEmpty(arrayObj);
            IsComplexType(arrayObj);

            Assert.AreEqual(SKONValueType.ARRAY, arrayObj.Type);

            Assert.IsTrue(arrayObj.Add("of"));
            Assert.IsTrue(arrayObj.Add("strings"));

            Assert.AreEqual("This", arrayObj[0].String);
            Assert.AreEqual("is", arrayObj[1].String);
            Assert.AreEqual("an", arrayObj[2].String);
            Assert.AreEqual("array", arrayObj[3].String);
            Assert.AreEqual("of", arrayObj[4].String);
            Assert.AreEqual("strings", arrayObj[5].String);

            Assert.Throws<IndexOutOfRangeException>(() => { arrayObj[6] = "!"; });

            ICollection<SKONObject> values = arrayObj.Values;

            values.Add("!");

            Assert.AreEqual("!", arrayObj[6].String);
            
            arrayObj.Remove((SKONObject)"!");

            Assert.Throws<IndexOutOfRangeException>(() => { arrayObj[6] = "!"; });
        }

        [Test]
        public void RecursiveObject()
        {
            SKONObject recursiveArray = SKONObject.GetEmptyArray();

            recursiveArray.Add(recursiveArray);

            Assert.DoesNotThrow(() => recursiveArray.Equals(recursiveArray));

            Assert.Throws<ArgumentException>(() => SKON.Write(new Dictionary<string, SKONObject> { { "Test", recursiveArray } }));

            recursiveArray.Add(new Dictionary<string, SKONObject> { { "Test2", recursiveArray } });

            Assert.DoesNotThrow(() => recursiveArray.Equals(recursiveArray[1]));

            // Equality between two recursive objects

            SKONObject rec1 = SKONObject.GetEmptyMap();

            rec1.Add("Rec", rec1);

            SKONObject rec2 = SKONObject.GetEmptyMap();

            rec2.Add("Rec", rec2);

            Assert.IsTrue(rec1.Equals(rec2));
        }

        [Test]
        public void NonReqursiveMapWithEqualMaps()
        {
            SKONObject skonObject = SKONObject.GetEmptyMap();

            skonObject.Add("test", new List<SKONObject> {
                new Dictionary<string, SKONObject> {
                    { "test", 1 }
                },
                new Dictionary<string, SKONObject> {
                    { "test", 1 }
                }
            });

            Assert.IsFalse(SKON.ContainsLoops(skonObject));

            Console.WriteLine(SKON.Write(skonObject));
        }
    }
}