//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

namespace SKON
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// All value types a SKONObject could be, including complex values.
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
        DATETIME,

        /// <summary>
        /// A (sub-) map value.
        /// </summary>
        MAP,

        /// <summary>
        /// An array value.
        /// </summary>
        ARRAY
    }

    /// <summary>
    /// A SKONObject. Every value in a SKON file is handled internally as a SKONObject.
    /// </summary>
    public class SKONObject : IEquatable<SKONObject>
    {
        /// <summary>
        /// Backing string value.
        /// </summary>
        private readonly string stringValue;

        /// <summary>
        /// Backing integer value.
        /// </summary>
        private readonly int intValue;

        /// <summary>
        /// Backing float value.
        /// </summary>
        private readonly double doubleValue;

        /// <summary>
        /// Backing boolean value.
        /// </summary>
        private readonly bool booleanValue;

        /// <summary>
        /// Backing DateTime value.
        /// </summary>
        private readonly DateTime dateTimeValue;

        /// <summary>
        /// Backing string-SKONObject dictionary of Map key-value pairs.
        /// </summary>
        private readonly Dictionary<string, SKONObject> mapValues;

        /// <summary>
        /// Backing array of Array values.
        /// </summary>
        private readonly List<SKONObject> arrayValues;

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs an empty SKONObject.
        /// </summary>
        public SKONObject()
        {
            this.Type = ValueType.EMPTY;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a string value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        public SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
            this.Type = ValueType.STRING;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an integer value.
        /// </summary>
        /// <param name="intValue">The integer value.</param>
        public SKONObject(int intValue)
        {
            this.intValue = intValue;
            this.Type = ValueType.INTEGER;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a double value.
        /// </summary>
        /// <param name="doubleValue">The double value.</param>
        public SKONObject(double doubleValue)
        {
            this.doubleValue = doubleValue;
            this.Type = ValueType.DOUBLE;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a boolean value.
        /// </summary>
        /// <param name="booleanValue">The boolean value.</param>
        public SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
            this.Type = ValueType.BOOLEAN;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a DateTime value.
        /// </summary>
        /// <param name="dateTimeValue">The DateTime value.</param>
        public SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
            this.Type = ValueType.DATETIME;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding a Map.
        /// </summary>
        /// <param name="mapValues">The key-value pairs inside that Map.</param>
        public SKONObject(Dictionary<string, SKONObject> mapValues)
        {
            this.mapValues = mapValues;
            this.Type = ValueType.MAP;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SKONObject"/> class.
        /// Constructs a SKONObject holding an Array.
        /// </summary>
        /// <param name="arrayValues">The SKONObject values making up that Array.</param>
        public SKONObject(IEnumerable<SKONObject> arrayValues)
        {
            this.arrayValues = new List<SKONObject>(arrayValues);
            this.Type = ValueType.ARRAY;
        }

        /// <summary>
        /// Gets a value indicating whether this SKONObject is empty or not.
        /// </summary>
        public bool IsEmpty => this.Type == ValueType.EMPTY;

        /// <summary>
        /// Gets the collection of string keys, if this SKONObject is a Map, or an empty ICollection, if it isn't.
        /// </summary>
        public List<string> Keys => this.Type == ValueType.MAP ? new List<string>(mapValues.Keys) : new List<string>();

        /// <summary>
        /// Gets the length of the array, should this SKONObject be one, or negative 1, if it isn't.
        /// </summary>
        public int Length => this.arrayValues?.Count ?? -1;

        /// <summary>
        /// Gets the type of this SKONObject.
        /// </summary>
        public ValueType Type { get; internal set; }

        /// <summary>
        /// Gets the string value of this SKONObject, should it be a string.
        /// </summary>
        public string String => this.Type == ValueType.STRING ? this.stringValue : null;

        /// <summary>
        /// Gets the integer value of this SKONObject, should it be an integer.
        /// </summary>
        public int? Int => this.Type == ValueType.INTEGER ? this.intValue : (int?) null;

        /// <summary>
        /// Gets the double value of this SKONObject, should it be a double.
        /// </summary>
        public double? Double => this.Type == ValueType.DOUBLE ? this.doubleValue : (double?) null;

        /// <summary>
        /// Gets the boolean value of this SKONObject, should it be a boolean.
        /// </summary>
        public bool? Boolean => this.Type == ValueType.BOOLEAN ? this.booleanValue : (bool?) null;

        /// <summary>
        /// Gets the DateTime value of this SKONObject, should it be a DateTime.
        /// </summary>
        public DateTime? DateTime => this.Type == ValueType.DATETIME ? this.dateTimeValue : (DateTime?) null;

        /// <summary>
        /// Gets the values of the SKONObject array.
        /// </summary>
        public List<SKONObject> Values => this.arrayValues ?? new List<SKONObject>();

        /// <summary>
        /// Gets an empty SKONObject.
        /// </summary>
        internal static SKONObject Empty => new SKONObject();

        /// <summary>
        /// Gets or sets the SKONObject value at the integer index i for a SKONObject Array.
        /// </summary>
        /// <param name="i">The index to get a SKONObject for.</param>
        /// <returns>The SKONObject found at the given index or an empty SKONObject, should the index be out of bounds.</returns>
        public SKONObject this[int i]
        {
            get
            {
                if (i >= 0 && i < this.arrayValues?.Count)
                {
                    return this.arrayValues[i];
                }

                return Empty;
            }

            set
            {
                if (this.Type == ValueType.ARRAY)
                {
                    if (i < 0 || i >= this.arrayValues?.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(i), i, "The given index was out of bounds!");
                    }

                    this.arrayValues[i] = value;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("This SKONObject is not an array! Index {0} cannot be set to value {1}!", i, value));
                }
            }
        }

        /// <summary>
        /// Gets the SKONObject value paired with the string key for a SKONObject Map.
        /// </summary>
        /// <param name="key">The paired string key to get a SKONObject for.</param>
        /// <returns>The SKONObject paired with the given key or an empty SKONObject, should the key be not in the list of keys.</returns>
        public SKONObject this[string key]
        {
            get
            {
                if (this.mapValues?.ContainsKey(key) ?? false)
                {
                    return this.mapValues[key];
                }

                return Empty;
            }

            set
            {
                if (this.Type == ValueType.MAP)
                {
                    this.mapValues[key] = value;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("This SKONObject is not a map! Adding value {0} to key {1} isn't possible!", value, key));
                }
            }
        }
        
        public static bool operator ==(SKONObject left, SKONObject right)
        {
            return (object)left != null ? left.Equals(right) : false;
        }

        public static bool operator !=(SKONObject left, SKONObject right)
        {
            return (object)left == null ? !left.Equals(right) : true;
        }

        /// <summary>
        /// Implicitly converts a string into a SKONObject.
        /// </summary>
        /// <param name="str">
        /// The string.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(string str) => new SKONObject(str);

        public static explicit operator string(SKONObject skon) => skon.String;

        /// <summary>
        /// Implicitly converts an integer into a SKONObject.
        /// </summary>
        /// <param name="i">
        /// The integer.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(int i) => new SKONObject(i);

        public static explicit operator int?(SKONObject skon) => skon.Int;

        /// <summary>
        /// Implicitly converts a double into a SKONObject.
        /// </summary>
        /// <param name="d">
        /// The double.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(double d) => new SKONObject(d);

        public static explicit operator double?(SKONObject skon) => skon.Double;

        /// <summary>
        /// Implicitly converts a boolean into a SKONObject.
        /// </summary>
        /// <param name="b">
        /// The boolean.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(bool b) => new SKONObject(b);

        public static explicit operator bool?(SKONObject skon) => skon.Boolean;

        /// <summary>
        /// Implicitly converts a DateTime into a SKONObject.
        /// </summary>
        /// <param name="dt">
        /// The DateTime.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(DateTime dt) => new SKONObject(dt);

        public static explicit operator DateTime?(SKONObject skon) => skon.DateTime;

        /// <summary>
        /// Implicitly converts a string array into a SKONObject Array of strings.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(string[] array)
        {
            return new SKONObject(new List<string>(array).ConvertAll((value) => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a string list into a SKONObject Array of strings.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(List<string> list)
        {
            return new SKONObject(list.ConvertAll(value => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts an integer array into a SKONObject Array of integers.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(int[] array)
        {
            return new SKONObject(new List<int>(array).ConvertAll((value) => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts an integer list into a SKONObject Array of integers.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(List<int> list)
        {
            return new SKONObject(list.ConvertAll(value => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a double array into a SKONObject Array of doubles.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(double[] array)
        {
            return new SKONObject(new List<double>(array).ConvertAll((value) => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a double list into a SKONObject Array of doubles.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(List<double> list)
        {
            return new SKONObject(list.ConvertAll(value => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a boolean array into a SKONObject Array of booleans.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(bool[] array)
        {
            return new SKONObject(new List<bool>(array).ConvertAll((value) => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a boolean list into a SKONObject Array of booleans.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(List<bool> list)
        {
            return new SKONObject(list.ConvertAll(value => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a DateTime array into a SKONObject Array of DateTimes.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(DateTime[] array)
        {
            return new SKONObject(new List<DateTime>(array).ConvertAll((value) => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a DateTime list into a SKONObject Array of DateTimes.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The newly converted SKONObject.
        /// </returns>
        public static implicit operator SKONObject(List<DateTime> list)
        {
            return new SKONObject(list.ConvertAll(value => (SKONObject)value));
        }

        /// <summary>
        /// Implicitly converts a Dictionary into a SKONObject Map.
        /// </summary>
        /// <param name="map"></param>
        public static implicit operator SKONObject(Dictionary<string, SKONObject> map)
        {
            return new SKONObject(map);
        }

        public void Add(string key, SKONObject value)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }
            
            mapValues[key] = value;
        }

        /// <summary>
        /// Adds a SKONObject to the Array.
        /// </summary>
        /// <param name="value">
        /// The value to add.
        /// </param>
        /// <returns>
        /// True if it succeeds, false if there is no array.
        /// </returns>
        public bool Add(SKONObject value)
        {
            if (this.Type == ValueType.ARRAY)
            {
                return false;
            }

            this.arrayValues.Add(value);
            return true;
        }

        /// <summary>
        /// Checks to see if this SKONObject contains the key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// True, if it exists, false if not or not a map.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.mapValues?.ContainsKey(key) ?? false;
        }

        /// <summary>
        /// Checks to see if this SKONObject contains all keys in the given array.
        /// </summary>
        /// <param name="keys">
        /// The keys.
        /// </param>
        /// <returns>
        /// True, if all exist, false if any are missing or not a map.
        /// </returns>
        public bool AllPresent(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (!this.ContainsKey(keys[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks to see if this SKONObject contains all keys in the given List of strings.
        /// </summary>
        /// <param name="keys">
        /// The keys.
        /// </param>
        /// <returns>
        /// True, if all exist, false if any are missing or not a map.
        /// </returns>
        public bool AllPresent(List<string> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (!this.ContainsKey(keys[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public string Get(string key, string defaultValue)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }
            
            return mapValues[key].String ?? defaultValue;
        }

        public int Get(string key, int defaultValue)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }

            return mapValues[key].Int ?? defaultValue;
        }

        public double Get(string key, double defaultValue)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }

            return mapValues[key].Double ?? defaultValue;
        }

        public bool Get(string key, bool defaultValue)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }

            return mapValues[key].Boolean ?? defaultValue;
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            if (this.Type != ValueType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }

            return mapValues[key].DateTime ?? defaultValue;
        }
        
        public bool TryGet(string key, out string result)
        {
            if (this.Type == ValueType.MAP && mapValues.ContainsKey(key))
            {
                if (mapValues[key].Type == ValueType.STRING)
                {
                    result = mapValues[key].String;
                    return true;
                }
            }
            
            result = default(string);
            return false;
        }

        public bool TryGet(string key, out int result)
        {
            if (this.Type == ValueType.MAP && mapValues.ContainsKey(key))
            {
                if (mapValues[key].Type == ValueType.INTEGER)
                {
                    // We acccess the value directly because we know it has a value
                    result = mapValues[key].intValue;
                    return true;
                }
            }

            result = default(int);
            return false;
        }

        public bool TryGet(string key, out double result)
        {
            if (this.Type == ValueType.MAP && mapValues.ContainsKey(key))
            {
                if (mapValues[key].Type == ValueType.DOUBLE)
                {
                    // We acccess the value directly because we know it has a value
                    result = mapValues[key].doubleValue;
                    return true;
                }
            }

            result = default(double);
            return false;
        }

        public bool TryGet(string key, out bool result)
        {
            if (this.Type == ValueType.MAP && mapValues.ContainsKey(key))
            {
                if (mapValues[key].Type == ValueType.BOOLEAN)
                {
                    // We acccess the value directly because we know it has a value
                    result = mapValues[key].booleanValue;
                    return true;
                }
            }

            result = default(bool);
            return false;
        }

        public bool TryGet(string key, out DateTime result)
        {
            if (this.Type == ValueType.MAP && mapValues.ContainsKey(key))
            {
                if (mapValues[key].Type == ValueType.DATETIME)
                {
                    // We acccess the value directly because we know it has a value
                    result = mapValues[key].dateTimeValue;
                    return true;
                }
            }

            result = default(DateTime);
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

        /// <summary>
        /// Determines if a SKONObject is equal to another SKONObject.
        /// </summary>
        /// <param name="other">The other SKONObject.</param>
        /// <returns>Wether or not the objects are equal.</returns>
        public bool Equals(SKONObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (this.Type != other.Type)
            {
                return false;
            }
            
            bool isEqual = true;

            switch (this.Type)
            {
                case ValueType.EMPTY:
                    return other.IsEmpty;
                case ValueType.STRING:
                    return this.String == other.String;
                case ValueType.INTEGER:
                    return this.Int == other.Int;
                case ValueType.DOUBLE:
                    return this.Double == other.Double;
                case ValueType.BOOLEAN:
                    return this.Boolean == other.Boolean;
                case ValueType.DATETIME:
                    return this.DateTime == other.DateTime;
                case ValueType.MAP:
                    if (this.Keys.Count != other.Keys.Count)
                    {
                        return false;
                    }

                    foreach (string key in this.Keys)
                    {
                        isEqual &= (this[key] == other[key]);
                    }

                    return isEqual;
                case ValueType.ARRAY:

                    if (this.Length != other.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < this.Length; i++)
                    {
                        isEqual &= (this[i] == other[i]);
                    }

                    return isEqual;
                default:
                    return false;
            }
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is SKONObject)
            {
                return this.Equals((SKONObject) obj);
            }

            return false;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                switch (this.Type)
                {
                    case ValueType.EMPTY:
                        return this.Type.GetHashCode();
                    case ValueType.STRING:
                        return this.Type.GetHashCode() * 17 + stringValue.GetHashCode();
                    case ValueType.INTEGER:
                        return this.Type.GetHashCode() * 17 + intValue.GetHashCode();
                case ValueType.DOUBLE:
                        return this.Type.GetHashCode() * 17 + doubleValue.GetHashCode();
                    case ValueType.BOOLEAN:
                        return this.Type.GetHashCode() * 17 + booleanValue.GetHashCode();
                    case ValueType.DATETIME:
                        return this.Type.GetHashCode() * 17 + dateTimeValue.GetHashCode();
                    case ValueType.MAP:
                        return this.Type.GetHashCode() * 17 + mapValues.GetHashCode();
                    case ValueType.ARRAY:
                        return this.Type.GetHashCode() * 17 + arrayValues.GetHashCode();
                    default:
                        return base.GetHashCode();
                }
            }
        }
    }
}
