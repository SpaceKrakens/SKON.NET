//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SKON
{
    public enum ValueType
    {
        EMPTY,
        STRING,
        INTEGER,
        DOUBLE,
        BOOLEAN,
        DATETIME,
        MAP,
        ARRAY
    }

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

        private Dictionary<string, SKONObject> mapValues;

        private SKONObject[] arrayValues;

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Dummy SKONObject constructor, probably not done and at this time not even used.
        /// </summary>
        internal SKONObject()
        {
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

        internal SKONObject(Dictionary<string, SKONObject> mapValues)
        {
            this.mapValues = mapValues;
            this.Type = ValueType.MAP;
        }

        internal SKONObject(SKONObject[] arrayValues)
        {
            this.arrayValues = arrayValues;
            this.Type = ValueType.ARRAY;
        }
        
        /// <summary>
        /// Gets a value indicating whether this SKONObject is empty or not.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Type == ValueType.EMPTY;
            }
        }
        
        public ICollection<string> Keys
        {
            get
            {
                return this.mapValues != null ? this.mapValues.Keys : null;
            }
        }

        public int Length
        {
            get
            {
                return this.arrayValues != null ? this.arrayValues.Length : -1;
            }
        }
        
        public ValueType Type { get; internal set; }

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

        public SKONObject this[int i]
        {
            get
            {
                if (i >= 0 && i < this.arrayValues.Length)
                {
                    return this.arrayValues[i];
                }
                else
                {
                    return Empty;
                }
            }
        }

        public SKONObject this[string key]
        {
            get
            {
                if (this.mapValues != null && this.mapValues.ContainsKey(key))
                {
                    return this.mapValues[key];
                }
                else
                {
                    return Empty;
                }
            }
        }
        
        /// <summary>
        /// Converts a SKONObject into a string.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static implicit operator string(SKONObject obj)
        {
            if (obj.Type != ValueType.STRING)
            {
                // Will this lead to weirdness? Always being able to cast to a string that will never be 'null'
                // Should string cast be explicit instead?
                return obj.ToString();
            }

            return obj.stringValue;
        }

        /// <summary>
        /// Converts a SKONObject into an integer.
        /// </summary>
        /// <param name="obj">The SKONObject to convert.</param>
        public static implicit operator int?(SKONObject obj)
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
        public static implicit operator double?(SKONObject obj)
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
        public static implicit operator bool?(SKONObject obj)
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
        public static implicit operator DateTime?(SKONObject obj)
        {
            if (obj.Type != ValueType.DATETIME)
            {
                return null;
            }

            return obj.dateTimeValue;
        }
        
        public T Get<T>(string key, T defaultValue)
        {
            if (this.mapValues != null && this.mapValues.ContainsKey(key))
            {
                System.Type sourceType = typeof(T);
                MethodInfo[] ops = sourceType.GetMethods();
                
                for (int i = 0; i < ops.Length; i++)
                {
                    MethodInfo op = ops[i];
                    if (op.ReturnType == typeof(T) && (op.Name == "op_Implicit" || op.Name == "op_Explicit"))
                    {
                        return (T) op.Invoke(null, new[] { this.mapValues[key] });
                    }
                }
            }

            return defaultValue;
        }
        
        public bool TryGet<T>(string key, T defaultValue, out T result)
        {
            if (this.mapValues != null && this.mapValues.ContainsKey(key))
            {
                System.Type sourceType = typeof(T);
                MethodInfo[] ops = sourceType.GetMethods();

                for (int i = 0; i < ops.Length; i++)
                {
                    MethodInfo op = ops[i];
                    if (op.ReturnType == typeof(T) && (op.Name == "op_Implicit" || op.Name == "op_Explicit"))
                    {
                        result = (T) op.Invoke(null, new[] { this.mapValues[key] });
                        return true;
                    }
                }
            }

            result = defaultValue;
            return false;
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
                case ValueType.MAP:
                    return this.mapValues.ToString();
                case ValueType.ARRAY:
                    return this.arrayValues.ToString();
                default:
                    return "Invalid type!";
            }
        }
    }
}
