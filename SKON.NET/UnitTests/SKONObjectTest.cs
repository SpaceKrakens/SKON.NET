using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SKON;

namespace UnitTests
{
    [TestFixture]
    class SKONObjectTest
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
        
        public static void Test_NotComplexType(SKONObject obj)
        {
            Assert.AreNotEqual(SKON.ValueType.MAP, obj.Type);
            Assert.AreNotEqual(SKON.ValueType.ARRAY, obj.Type);

            Assert.AreEqual(-1, obj.Length);
            Assert.AreEqual(0, obj.Values.Count);
            Assert.AreEqual(0, obj.Keys.Count);

            Assert.IsTrue(obj[TestKey].IsEmpty);
            Assert.IsTrue(obj[0].IsEmpty);
        }

        public static void Test_NotSimpleType(SKONObject obj)
        {
            Assert.AreNotEqual(SKON.ValueType.STRING, obj.Type);
            Assert.AreNotEqual(SKON.ValueType.INTEGER, obj.Type);
            Assert.AreNotEqual(SKON.ValueType.DOUBLE, obj.Type);
            Assert.AreNotEqual(SKON.ValueType.BOOLEAN, obj.Type);
            Assert.AreNotEqual(SKON.ValueType.DATETIME, obj.Type);
            
            Assert.IsNull(obj.String);
            Assert.IsNull(obj.Int);
            Assert.IsNull(obj.Double);
            Assert.IsNull(obj.Boolean);
            Assert.IsNull(obj.DateTime);
        }

        public static void Test_IsEmpty(SKONObject emptyObj)
        {
            Assert.AreEqual(SKON.ValueType.EMPTY, emptyObj.Type, "Empty SKONObject.Type is not EMPTY!");

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

        public static void Test_IsNotEmpty(SKONObject obj)
        {
            Assert.IsFalse(obj.IsEmpty);
            
            bool hasValue = false;

            hasValue |= obj.String != null;
            hasValue |= obj.Int != null;
            hasValue |= obj.Double != null;
            hasValue |= obj.Boolean != null;
            hasValue |= obj.DateTime != null;

            hasValue |= obj.Keys.Count > 0;
            hasValue |= obj.Values.Count > 0;

            Assert.IsTrue(hasValue, "SKONObject does not contain any value!");
        }

        public static void TestObject(string expected, SKONObject actual)
        {
            Assert.AreEqual(SKON.ValueType.STRING, actual.Type);

            Test_IsNotEmpty(actual);

            Test_NotComplexType(actual);

            Assert.AreEqual(expected, actual.String);
        }

        public static void TestObject(int expected, SKONObject actual)
        {
            Assert.AreEqual(SKON.ValueType.INTEGER, actual.Type);

            Test_IsNotEmpty(actual);

            Test_NotComplexType(actual);

            Assert.AreEqual(expected, actual.Int);
        }

        public void TestObject(double expected, SKONObject actual)
        {
            Assert.AreEqual(SKON.ValueType.DOUBLE, actual.Type);

            Test_IsNotEmpty(actual);

            Test_NotComplexType(actual);

            Assert.AreEqual(expected, actual.Double);
        }

        public void TestObject(bool expected, SKONObject actual)
        {
            Assert.AreEqual(SKON.ValueType.BOOLEAN, actual.Type);

            Test_IsNotEmpty(actual);

            Test_NotComplexType(actual);

            Assert.AreEqual(expected, actual.Boolean);
        }

        public void TestObject(DateTime expected, SKONObject actual)
        {
            Assert.AreEqual(SKON.ValueType.DATETIME, actual.Type);

            Test_IsNotEmpty(actual);

            Test_NotComplexType(actual);
            
            Assert.AreEqual(expected, actual.DateTime);
        }
        
        [Test]
        public void Test_EmptyValue()
        {
            Test_IsEmpty(new SKONObject());
        }

        [Test]
        public void Test_StringValue()
        {
            SKONObject stringObj = new SKONObject(TestString);

            Assert.AreEqual(SKON.ValueType.STRING, stringObj.Type);

            Test_IsNotEmpty(stringObj);

            Assert.IsNull(stringObj.Int);
            Assert.IsNull(stringObj.Double);
            Assert.IsNull(stringObj.Boolean);
            Assert.IsNull(stringObj.DateTime);

            Assert.AreEqual(TestString, stringObj.String);

            Test_NotComplexType(stringObj);
        }

        [Test]
        public void Test_IntValue()
        {
            SKONObject intObj = new SKONObject(TestInt);

            Assert.AreEqual(SKON.ValueType.INTEGER, intObj.Type);

            Test_IsNotEmpty(intObj);

            Assert.IsNull(intObj.String);
            Assert.IsNull(intObj.Double);
            Assert.IsNull(intObj.Boolean);
            Assert.IsNull(intObj.DateTime);
            
            Assert.AreEqual(TestInt, intObj.Int);

            Test_NotComplexType(intObj);
        }

        [Test]
        public void Test_DoubleValue()
        {
            SKONObject doubleObj = new SKONObject(TestDouble);

            Assert.AreEqual(SKON.ValueType.DOUBLE, doubleObj.Type);

            Test_IsNotEmpty(doubleObj);

            Assert.IsNull(doubleObj.String);
            Assert.IsNull(doubleObj.Int);
            Assert.IsNull(doubleObj.Boolean);
            Assert.IsNull(doubleObj.DateTime);

            Assert.AreEqual(TestDouble, doubleObj.Double);

            Test_NotComplexType(doubleObj);
        }

        [Test]
        public void Test_BooleanValue()
        {
            SKONObject booleanObj = new SKONObject(TestBoolean);

            Assert.AreEqual(SKON.ValueType.BOOLEAN, booleanObj.Type);

            Test_IsNotEmpty(booleanObj);

            Assert.IsNull(booleanObj.String);
            Assert.IsNull(booleanObj.Int);
            Assert.IsNull(booleanObj.Double);
            Assert.IsNull(booleanObj.DateTime);

            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            Test_NotComplexType(booleanObj);
        }

        [Test]
        public void Test_DateTimeValue()
        {
            SKONObject dateTimeObj = new SKONObject(TestDateTime);

            Assert.AreEqual(SKON.ValueType.DATETIME, dateTimeObj.Type);

            Test_IsNotEmpty(dateTimeObj);

            Assert.IsNull(dateTimeObj.String);
            Assert.IsNull(dateTimeObj.Int);
            Assert.IsNull(dateTimeObj.Double);
            Assert.IsNull(dateTimeObj.Boolean);

            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);

            Test_NotComplexType(dateTimeObj);
        }

        [Test]
        public void Test_MapValue()
        {
            SKONObject mapObj = TestMap;

            Assert.AreEqual(SKON.ValueType.MAP, mapObj.Type);

            Test_IsNotEmpty(mapObj);

            Test_NotSimpleType(mapObj);
            
            Assert.IsTrue(mapObj[TestKey].IsEmpty);
            
            Assert.AreEqual(6, mapObj.Keys.Count);
            
            Assert.IsTrue(mapObj.AllPresent("Empty", "String", "Int", "Double", "Bool", "DateTime"));
            Assert.IsFalse(mapObj.AllPresent(TestKey));
            
            SKONObject emptyObj = mapObj["Empty"];
            Test_IsEmpty(emptyObj);

            SKONObject stringObj = mapObj["String"];
            Test_IsNotEmpty(stringObj);
            Assert.AreEqual(TestString, stringObj.String);

            SKONObject intObj = mapObj["Int"];
            Test_IsNotEmpty(intObj);
            Assert.AreEqual(TestInt, intObj.Int);

            SKONObject doubleObj = mapObj["Double"];
            Test_IsNotEmpty(doubleObj);
            Assert.AreEqual(TestDouble, doubleObj.Double);

            SKONObject booleanObj = mapObj["Bool"];
            Test_IsNotEmpty(booleanObj);
            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            SKONObject dateTimeObj = mapObj["DateTime"];
            Test_IsNotEmpty(dateTimeObj);
            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);
        }

        [Test]
        public void Test_ListValue()
        {
            SKONObject listObj = TestStringList;

            Assert.AreEqual(SKON.ValueType.ARRAY, listObj.Type);

            Test_IsNotEmpty(listObj);

            Test_NotSimpleType(listObj);

            Assert.AreNotEqual(-1, listObj.Length);

            Assert.AreEqual(6, listObj.Values.Count);

            SKONObject emptyObj = listObj[0];
            Test_IsEmpty(emptyObj);
            Test_NotComplexType(emptyObj);
            Test_NotSimpleType(emptyObj);

            SKONObject stringObj = listObj[1];
            Test_IsNotEmpty(stringObj);
            Test_NotComplexType(stringObj);
            Assert.AreEqual(TestString, stringObj.String);

            SKONObject intObj = listObj[2];
            Test_IsNotEmpty(intObj);
            Test_NotComplexType(intObj);
            Assert.AreEqual(TestInt, intObj.Int);

            SKONObject doubleObj = listObj[3];
            Test_IsNotEmpty(doubleObj);
            Test_NotComplexType(doubleObj);
            Assert.AreEqual(TestDouble, doubleObj.Double);

            SKONObject booleanObj = listObj[4];
            Test_IsNotEmpty(booleanObj);
            Test_NotComplexType(booleanObj);
            Assert.AreEqual(TestBoolean, booleanObj.Boolean);

            SKONObject dateTimeObj = listObj[5];
            Test_IsNotEmpty(dateTimeObj);
            Test_NotComplexType(dateTimeObj);
            Assert.AreEqual(TestDateTime, dateTimeObj.DateTime);
        }
    }
}