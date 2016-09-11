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
    public enum Type
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
            Type = Type.EMPTY;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a string value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        internal SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
            Type = Type.STRING;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an integer value.
        /// </summary>
        /// <param name="intValue">The integer value.</param>
        internal SKONObject(int intValue)
        {
            this.intValue = intValue;
            Type = Type.INTEGER;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a float value.
        /// </summary>
        /// <param name="floatValue">The float value.</param>
        internal SKONObject(double doubleValue)
        {
            this.doubleValue = doubleValue;
            Type = Type.DOUBLE;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a boolean value.
        /// </summary>
        /// <param name="booleanValue">The boolean value.</param>
        internal SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
            Type = Type.BOOLEAN;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a DateTime value.
        /// </summary>
        /// <param name="dateTimeValue">The DateTime value.</param>
        internal SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
            Type = Type.DATETIME;
        }

        internal SKONObject(Dictionary<string, SKONObject> mapValues)
        {
            this.mapValues = mapValues;
            Type = Type.MAP;
        }

        internal SKONObject(SKONObject[] arrayValues)
        {
            this.arrayValues = arrayValues;
            Type = Type.ARRAY;
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

        public Type Type { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this SKONObject is empty or not.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Type == Type.EMPTY;
            }
        }
        
        public ICollection<string> Keys
        {
            get
            {
                return mapValues != null ? mapValues.Keys : null;
            }
        }

        public int Length
        {
            get
            {
                return arrayValues != null ? arrayValues.Length : -1;
            }
        }

        public SKONObject this[int i]
        {
            get
            {
                if (i >= 0 && i < arrayValues.Length)
                {
                    return arrayValues[i];
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
                if (mapValues != null && mapValues.ContainsKey(key))
                {
                    return mapValues[key];
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
            if (obj.Type != Type.STRING)
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
            if (obj.Type != Type.INTEGER)
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
            if (obj.Type != Type.DOUBLE)
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
            if (obj.Type != Type.BOOLEAN)
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
            if (obj.Type != Type.DATETIME)
            {
                return null;
            }
            return obj.dateTimeValue;
        }
        
        public T Get<T>(string key, T defaultValue)
        {
            if (mapValues != null && mapValues.ContainsKey(key))
            {
                System.Type sourceType = typeof(T);
                MethodInfo[] ops = sourceType.GetMethods();
                
                for (int i = 0; i < ops.Length; i++)
                {
                    MethodInfo op = ops[i];
                    if (op.ReturnType == typeof(T) && (op.Name == "op_Implicit" || op.Name == "op_Explicit"))
                    {
                        return (T) op.Invoke(null, new[] { mapValues[key] });
                    }
                }
            }

            return defaultValue;
        }
        
        public bool TryGet<T>(string key, T defaultValue, out T result)
        {
            if (mapValues != null && mapValues.ContainsKey(key))
            {
                System.Type sourceType = typeof(T);
                MethodInfo[] ops = sourceType.GetMethods();

                for (int i = 0; i < ops.Length; i++)
                {
                    MethodInfo op = ops[i];
                    if (op.ReturnType == typeof(T) && (op.Name == "op_Implicit" || op.Name == "op_Explicit"))
                    {
                        result = (T) op.Invoke(null, new[] { mapValues[key] });
                        return true;
                    }
                }
            }

            result = defaultValue;
            return false;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case Type.EMPTY:
                    return "Empty";
                case Type.STRING:
                    return stringValue;
                case Type.INTEGER:
                    return "int: " + intValue.ToString();
                case Type.DOUBLE:
                    return "double: " + doubleValue.ToString();
                case Type.BOOLEAN:
                    return "bool: " + booleanValue.ToString();
                case Type.DATETIME:
                    return "datetime: " + dateTimeValue.ToString();
                default:
                    return "Invalid type!";
            }
        }
    }
}
