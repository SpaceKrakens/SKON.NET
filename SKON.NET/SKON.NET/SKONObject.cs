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

        public enum Type
        {
            EMPTY,
            STRING,
            INTEGER,
            DOUBLE,
            BOOLEAN,
            DATETIME
        }

        public readonly Type type;

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
            IsEmpty = true;
            type = Type.EMPTY;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a string value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        internal SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
            type = Type.STRING;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an integer value.
        /// </summary>
        /// <param name="intValue">The integer value.</param>
        internal SKONObject(int intValue)
        {
            this.intValue = intValue;
            type = Type.INTEGER;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a float value.
        /// </summary>
        /// <param name="floatValue">The float value.</param>
        internal SKONObject(double doubleValue)
        {
            this.doubleValue = doubleValue;
            type = Type.DOUBLE;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a boolean value.
        /// </summary>
        /// <param name="booleanValue">The boolean value.</param>
        internal SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
            type = Type.BOOLEAN;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a DateTime value.
        /// </summary>
        /// <param name="dateTimeValue">The DateTime value.</param>
        internal SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
            type = Type.DATETIME;
        }

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
        /// Gets a value indicating whether this SKONObject is empty or not.
        /// </summary>
        public bool IsEmpty { get; internal set; }

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
            if (obj.type != Type.STRING)
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
            if (obj.type != Type.INTEGER)
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
            if (obj.type != Type.DOUBLE)
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
            if (obj.type != Type.BOOLEAN)
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
            if (obj.type != Type.DATETIME)
            {
                return null;
            }
            return obj.dateTimeValue;
        }

        public override string ToString()
        {
            switch (type)
            {
                case Type.EMPTY:
                    return "Empty";
                case Type.STRING:
                    return stringValue;
                case Type.INTEGER:
                    return intValue.ToString();
                case Type.DOUBLE:
                    return doubleValue.ToString();
                case Type.BOOLEAN:
                    return booleanValue.ToString();
                case Type.DATETIME:
                    return dateTimeValue.ToString();
                default:
                    return "Invalid type!";
            }
        }
    }
}
