#region LICENCE
//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace SKON.SKEMA
{
    using System;
    using System.Collections.Generic;

    public enum SKEMAType
    {
        REFERENCE = -1,
        ANY,
        STRING,
        INTEGER,
        FLOAT,
        BOOLEAN,
        DATETIME,
        MAP,
        ARRAY
    }

    public class SKEMAObject : IEquatable<SKEMAObject>
    {
        public static SKEMAObject Any => new SKEMAObject(SKEMAType.ANY);

        public static SKEMAObject String => new SKEMAObject(SKEMAType.STRING);

        public static SKEMAObject Integer => new SKEMAObject(SKEMAType.INTEGER);

        public static SKEMAObject Float => new SKEMAObject(SKEMAType.FLOAT);

        public static SKEMAObject Boolean => new SKEMAObject(SKEMAType.BOOLEAN);

        public static SKEMAObject DateTime => new SKEMAObject(SKEMAType.DATETIME);

        public static SKEMAObject ArrayOf(SKEMAObject obj) => new SKEMAObject(obj);

        public static SKEMAObject AsReference(SKEMAObject obj, string definitionName) => new SKEMAObject(definitionName, obj);

        private readonly SKEMAType type;
        
        private Dictionary<string, SKEMAObject> mapSKEMA;
        private Dictionary<string, bool> optionalMap;
        
        private SKEMAObject arraySKEMA;
        public SKEMAObject ArrayElementSKEMA
        {
            get
            {
                return this.Type == SKEMAType.ARRAY ? arraySKEMA : null;
            }
            set
            {
                if (this.Type != SKEMAType.ARRAY)
                {
                    throw new InvalidOperationException("Only SKEMAObjects of type array can write to the ArrayElementSKEMA property!");
                }

                arraySKEMA = value;
            }
        }

        private string reference;
        public string Reference => reference;
        
        public SKEMAObject ReferenceSKEMA { get; internal set; }

        private SKEMAObject(SKEMAType type)
        {
            this.type = type;

            if (this.type == SKEMAType.MAP)
            {
                mapSKEMA = new Dictionary<string, SKEMAObject>();
                optionalMap = new Dictionary<string, bool>();
            }
            else if (this.type == SKEMAType.ARRAY)
            {
                arraySKEMA = new SKEMAObject(SKEMAType.ANY);
            }
        }

        public SKEMAObject(Dictionary<string, SKEMAObject> mapSKEMA, Dictionary<string, bool> optionalMap = null)
        {
            this.type = SKEMAType.MAP;

            this.mapSKEMA = mapSKEMA ?? new Dictionary<string, SKEMAObject>();
            this.optionalMap = optionalMap ?? new Dictionary<string, bool>();
        }

        private SKEMAObject(SKEMAObject arraySKEMA)
        {
            type = SKEMAType.ARRAY;

            this.arraySKEMA = arraySKEMA;
        }

        internal SKEMAObject(string reference)
        {
            this.type = SKEMAType.REFERENCE;
            this.reference = reference;
        }

        internal SKEMAObject(string reference, SKEMAObject definition)
        {
            this.type = SKEMAType.REFERENCE;
            this.reference = reference;
            ReferenceSKEMA = definition;
        }

        public SKEMAType Type => type;

        public List<string> Keys => new List<string>(mapSKEMA.Keys);

        public SKEMAObject this[string key]
        {
            get
            {
                return Get(key, Any);
            }

            set
            {
                Add(key, value);
            }
        }

        public bool IsOptional(string key) => optionalMap.ContainsKey(key);

        public void SetOptional(string key, bool optional)
        {
            if (type == SKEMAType.MAP)
            {
                if (optional == true)
                {
                    optionalMap[key] = true;
                }
                else if (optionalMap.ContainsKey(key))
                {
                    optionalMap.Remove(key);
                }
            }
        }

        public static bool operator ==(SKEMAObject left, SKEMAObject right)
        {
            return left?.Equals(right) ?? ((object)right == null);
        }

        public static bool operator !=(SKEMAObject left, SKEMAObject right)
        {
            return !(left == right);
        }

        public static implicit operator SKEMAObject(SKEMAType type) => new SKEMAObject(type);

        public static implicit operator SKEMAObject(Dictionary<string, SKEMAObject> mapSKEMA) => new SKEMAObject(mapSKEMA);

        public bool Optional { get; set; }

        public void Add(string key, SKEMAObject value)
        {
            if (this.Type != SKEMAType.MAP)
            {
                throw new InvalidOperationException(string.Format("This SKONObject is not a MAP! Adding value {0} to key {1} isn't possible!", value, key));
            }

            this.mapSKEMA[key] = value;
        }

        public bool Remove(string key)
        {
            if (this.Type != SKEMAType.MAP)
            {
                return false;
            }

            return mapSKEMA.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return this.mapSKEMA?.ContainsKey(key) ?? false;
        }

        public bool ContainsAllKeys(params string[] keys)
        {
            if (this.Type != SKEMAType.MAP)
            {
                return false;
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (this.ContainsKey(keys[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainsAllKeys(IEnumerable<string> keys)
        {
            if (this.Type != SKEMAType.MAP)
            {
                return false;
            }

            foreach (string key in keys)
            {
                if (mapSKEMA.ContainsKey(key) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainsAllKeys(List<string> keys)
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

        public SKEMAObject Get(string key, SKEMAObject defaultValue)
        {
            if (this.Type != SKEMAType.MAP)
            {
                throw new InvalidOperationException("This method only works on MAP type SKONObjects!");
            }

            return mapSKEMA[key] ?? defaultValue;
        }

        public bool TryGet(string key, out SKEMAObject result)
        {
            if (this.Type == SKEMAType.MAP && mapSKEMA.ContainsKey(key))
            {
                result = mapSKEMA[key];
                return true;
            }

            result = default(SKEMAObject);
            return false;
        }

        public bool Valid(SKONObject obj)
        {
            if (this.type != SKEMAType.REFERENCE && this.type != SKEMAType.ANY && (int)this.type != (int)obj.Type)
            {
                return false;
            }

            switch (this.type)
            {
                case SKEMAType.REFERENCE:
                    if (ReferenceSKEMA == null)
                    {
                        throw new InvalidOperationException("Trying to validate SKEMA without fully resolved references!");
                    }

                    return ReferenceSKEMA.Valid(obj);
                case SKEMAType.ANY:
                case SKEMAType.STRING:
                case SKEMAType.INTEGER:
                case SKEMAType.FLOAT:
                case SKEMAType.BOOLEAN:
                case SKEMAType.DATETIME:
                    return true;
                case SKEMAType.MAP:
                    if (optionalMap == null)
                    {
                        if (mapSKEMA.Count != obj.Keys.Count)
                        {
                            return false;
                        }

                        foreach (string key in mapSKEMA.Keys)
                        {
                            if (obj.ContainsKey(key) == false)
                            {
                                return false;
                            }

                            if (mapSKEMA[key].Valid(obj[key]) == false)
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        if (obj.Keys.Count > mapSKEMA.Count || obj.Keys.Count < mapSKEMA.Count - optionalMap?.Count)
                        {
                            return false;
                        }

                        if (this.ContainsAllKeys(obj.Keys) == false)
                        {
                            return false;
                        }

                        foreach (string key in mapSKEMA.Keys)
                        {
                            if (obj.ContainsKey(key) == false)
                            {
                                if (optionalMap.ContainsKey(key))
                                {
                                    continue;
                                }

                                return false;
                            }

                            if (mapSKEMA[key].Valid(obj[key]) == false)
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                case SKEMAType.ARRAY:
                    foreach (SKONObject value in obj.Values)
                    {
                        if (arraySKEMA?.Valid(value) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }

        //FIXME! Equals method is not working well
        // It can requrse
        
        public bool Equals(SKEMAObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Type != other.Type)
            {
                return false;
            }

            bool isEqual = true;

            switch (this.Type)
            {
                case SKEMAType.REFERENCE:
                    if (this.Reference != other.Reference)
                    {
                        return false;
                    }
                    
                    return this.ReferenceSKEMA?.Equals(other.ReferenceSKEMA) ?? (other.ReferenceSKEMA == null);
                case SKEMAType.ANY:
                case SKEMAType.STRING:
                case SKEMAType.INTEGER:
                case SKEMAType.FLOAT:
                case SKEMAType.BOOLEAN:
                case SKEMAType.DATETIME:
                    return true;
                case SKEMAType.MAP:
                    if (this.Keys.Count != other.Keys.Count)
                    {
                        return false;
                    }

                    foreach (string key in this.Keys)
                    {
                        isEqual &= (this[key].Equals(other[key]));
                    }

                    return isEqual;
                case SKEMAType.ARRAY:
                    return ArrayElementSKEMA.Equals(other.ArrayElementSKEMA);
                default:
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
            {
                return false;
            }

            if (obj is SKEMAObject)
            {
                return this.Equals((SKEMAObject)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                switch (this.Type)
                {
                    case SKEMAType.REFERENCE:
                        return ((this.Type.GetHashCode() * 17) + Reference.GetHashCode()) * 17;
                    case SKEMAType.ANY:
                    case SKEMAType.STRING:
                    case SKEMAType.INTEGER:
                    case SKEMAType.FLOAT:
                    case SKEMAType.BOOLEAN:
                    case SKEMAType.DATETIME:
                        return this.Type.GetHashCode();
                    case SKEMAType.MAP:
                        return this.Type.GetHashCode() * 17 + mapSKEMA.GetHashCode();
                    case SKEMAType.ARRAY:
                        // We don't do GetHashCode() directly to avoid recursion.
                        return this.Type.GetHashCode() * 17 + ArrayElementSKEMA.Type.GetHashCode();
                    default:
                        return base.GetHashCode();
                }
            }
        }
    }
}