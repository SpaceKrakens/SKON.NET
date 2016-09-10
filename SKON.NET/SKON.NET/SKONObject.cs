//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SKON.NET
{
    /// <summary>
    /// A SKONObject. Every value in a SKON file is handled internally as a SKONObject.
    /// </summary>
    public class SKONObject
    {
        /// <summary>
        /// The type of this SKONObject.
        /// </summary>
        public readonly ValueType Type;

        /// <summary>
        /// Backing string value.
        /// </summary>
        private string stringValue;

        /// <summary>
        /// Backing integer value.
        /// </summary>
        private int intValue;

        /// <summary>
        /// Backing float value.
        /// </summary>
        private double doubleValue;

        /// <summary>
        /// Backing boolean value.
        /// </summary>
        private bool booleanValue;

        /// <summary>
        /// Backing DateTime value.
        /// </summary>
        private DateTime dateTimeValue;

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Dummy SKONObject constructor, probably not done and at this time not even used.
        /// </summary>
        internal SKONObject()
        {
            this.IsEmpty = true;
            this.Type = ValueType.EMPTY;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a string value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        internal SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
            this.Type = ValueType.STRING;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an integer value.
        /// </summary>
        /// <param name="intValue">The integer value.</param>
        internal SKONObject(int intValue)
        {
            this.intValue = intValue;
            this.Type = ValueType.INTEGER;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a double value.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        internal SKONObject(double doubleValue)
        {
            this.doubleValue = doubleValue;
            this.Type = ValueType.DOUBLE;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a boolean value.
        /// </summary>
        /// <param name="booleanValue">The boolean value.</param>
        internal SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
            this.Type = ValueType.BOOLEAN;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a DateTime value.
        /// </summary>
        /// <param name="dateTimeValue">The DateTime value.</param>
        internal SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
            this.Type = ValueType.DATETIME;
        }

        /// <summary>
        /// All SKONObject value types.
        /// </summary>
        public enum ValueType
        {
            /// <summary>
            /// An empty value.
            /// </summary>
            EMPTY,

            /// <summary>
            /// A string value.
            /// </summary>
            STRING,

            /// <summary>
            /// An integer value.
            /// </summary>
            INTEGER,

            /// <summary>
            /// A double value.
            /// </summary>
            DOUBLE,

            /// <summary>
            /// A boolean value.
            /// </summary>
            BOOLEAN,

            /// <summary>
            /// A DateTime value.
            /// </summary>
            DATETIME
        }

        /// <summary>
        /// Gets a value indicating whether this SKONObject is empty or not.
        /// </summary>
        public bool IsEmpty { get; internal set; }

        /// <summary>
        /// Gets an empty SKONObject.
        /// </summary>
        internal static SKONObject Empty
        {
            get
            {
                return new SKONObject();
            }
        }

        /// <summary>
        /// Dummy implementation of an Array accessor. Should never be called by itself.
        /// </summary>
        /// <param name="i">The index to get a value for.</param>
        /// <returns>Null, since SKONObject does not implement Array behaviour.</returns>
        public virtual SKONObject this[int i]
        {
            get
            {
                return Empty;
            }
        }

        /// <summary>
        /// Dummy implementation of a Map accessor. Should never be called by itself.
        /// </summary>
        /// <param name="key">The key to get a value for.</param>
        /// <returns>Null, since SKONObject does not implement Map behaviour.</returns>
        public virtual SKONObject this[string key]
        {
            get
            {
                return Empty;
            }
        }

        /// <summary>
        /// Converts a SKONObject into a string.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator string(SKONObject obj)
        {
            if (obj.Type != ValueType.STRING)
            {
                return null;
            }

            return obj.stringValue;
        }

        /// <summary>
        /// Converts a SKONObject into an integer.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator int?(SKONObject obj)
        {
            if (obj.Type != ValueType.INTEGER)
            {
                return null;
            }

            return obj.intValue;
        }

        /// <summary>
        /// Converts a SKONObject into a float.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator double?(SKONObject obj)
        {
            if (obj.Type != ValueType.DOUBLE)
            {
                return null;
            }

            return obj.doubleValue;
        }

        /// <summary>
        /// Converts a SKONObject into a boolean.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator bool?(SKONObject obj)
        {
            if (obj.Type != ValueType.BOOLEAN)
            {
                return null;
            }

            return obj.booleanValue;
        }

        /// <summary>
        /// Converts a SKONObject into a DateTime.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator DateTime?(SKONObject obj)
        {
            if (obj.Type != ValueType.DATETIME)
            {
                return null;
            }

            return obj.dateTimeValue;
        }

        /// <summary>
        /// Returns the value of this SKONObject as a string.
        /// </summary>
        /// <returns>A string description of this SKONObject.</returns>
        public override string ToString()
        {
            switch (this.Type)
            {
                case ValueType.EMPTY:
                    return "Empty";
                case ValueType.STRING:
                    return this.stringValue;
                case ValueType.INTEGER:
                    return this.intValue.ToString();
                case ValueType.DOUBLE:
                    return this.doubleValue.ToString();
                case ValueType.BOOLEAN:
                    return this.booleanValue.ToString();
                case ValueType.DATETIME:
                    return this.dateTimeValue.ToString();
                default:
                    return "Invalid type!";
            }
        }
    }
}
