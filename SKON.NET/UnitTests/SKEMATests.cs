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

    [TestFixture]
    class SKEMATests
    {
        [Test]
        public void AnySKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.ANY).Valid(SKONObjectTests.TestMap));
        }

        [Test]
        public void WrongTypeSKEMA()
        {
            Assert.IsFalse(new SKEMAObject(SKEMAType.STRING).Valid(SKONObjectTests.TestInt));
        }

        [Test]
        public void StringSKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.STRING).Valid(SKONObjectTests.TestString));
        }

        [Test]
        public void IntegerSKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.INTEGER).Valid(SKONObjectTests.TestInt));
        }

        [Test]
        public void DoubleSKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.DOUBLE).Valid(SKONObjectTests.TestDouble));
        }

        [Test]
        public void BooleanSKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.BOOLEAN).Valid(SKONObjectTests.TestBoolean));
        }

        [Test]
        public void DateTimeSKEMA()
        {
            Assert.IsTrue(new SKEMAObject(SKEMAType.DATETIME).Valid(SKONObjectTests.TestDateTime));
        }
    }
}
