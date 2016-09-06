//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKraken">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKON.NET
{
    /// <summary>
    /// A SKONObject. Every value in a SKON file is handled internally as a SKONObject.
    /// </summary>
    public class SKONObject
    {
        /// <summary>
        /// Backing string value.
        /// </summary>
        private string stringValue;

        /// <summary>
        /// Backing integer value.
        /// </summary>
        private int? intValue;

        /// <summary>
        /// Backing float value.
        /// </summary>
        private float? floatValue;

        /// <summary>
        /// Backing boolean value.
        /// </summary>
        private bool? booleanValue;

        /// <summary>
        /// Backing DateTime value.
        /// </summary>
        private DateTime? dateTimeValue;

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Dummy SKONObject constructor, probably not done and at this time not even used.
        /// </summary>
        internal SKONObject()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a string value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        internal SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an integer value.
        /// </summary>
        /// <param name="intValue">The integer value.</param>
        internal SKONObject(int intValue)
        {
            this.intValue = intValue;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a float value.
        /// </summary>
        /// <param name="floatValue">The float value.</param>
        internal SKONObject(float floatValue)
        {
            this.floatValue = floatValue;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a boolean value.
        /// </summary>
        /// <param name="booleanValue">The boolean value.</param>
        internal SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a DateTime value.
        /// </summary>
        /// <param name="dateTimeValue">The DateTime value.</param>
        internal SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
        }

        /// <summary>
        /// Gets an empty SKONObject.
        /// </summary>
        public static SKONObject Empty
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
            return obj.stringValue;
        }

        /// <summary>
        /// Converts a SKONObject into an integer.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator int?(SKONObject obj)
        {
            return obj.intValue;
        }

        /// <summary>
        /// Converts a SKONObject into a float.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator float?(SKONObject obj)
        {
            return obj.floatValue;
        }

        /// <summary>
        /// Converts a SKONObject into a boolean.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator bool?(SKONObject obj)
        {
            return obj.booleanValue;
        }

        /// <summary>
        /// Converts a SKONObject into a DateTime.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static explicit operator DateTime?(SKONObject obj)
        {
            return obj.dateTimeValue;
        }
    }
}
