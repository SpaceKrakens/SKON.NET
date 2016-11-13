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
    using System.Text;
    using System.Text.RegularExpressions;
    using ValueType = SKONValueType;
    
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

    public class SKEMAObject
    {
        public static SKEMAObject Any => new SKEMAObject(SKEMAType.ANY);

        public static SKEMAObject String => new SKEMAObject(SKEMAType.STRING);

        public static SKEMAObject Integer => new SKEMAObject(SKEMAType.INTEGER);

        public static SKEMAObject Float => new SKEMAObject(SKEMAType.FLOAT);

        public static SKEMAObject Boolean => new SKEMAObject(SKEMAType.BOOLEAN);

        public static SKEMAObject DateTime => new SKEMAObject(SKEMAType.DATETIME);

        public static SKEMAObject ArrayOf(SKEMAObject obj) => new SKEMAObject(obj);

        private readonly SKEMAType type;

        private Dictionary<string, SKEMAObject> mapSKEMA;

        private bool loose;
        public bool Loose {
            get
            {
                if (Type != SKEMAType.MAP)
                {
                    throw new InvalidOperationException("Only SKEMAObjects of type MAP can be loose!");
                }
                return loose;
            }
            set
            {
                if (Type != SKEMAType.MAP)
                {
                    throw new InvalidOperationException("Only SKEMAObjects of type MAP can be loose!");
                }
                loose = value;
            }
        }

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

        private SKEMAObject(SKEMAType type)
        {
            this.type = type;

            if (this.type == SKEMAType.MAP)
            {
                mapSKEMA = new Dictionary<string, SKEMAObject>();
            }
            else if (this.type == SKEMAType.ARRAY)
            {
                arraySKEMA = new SKEMAObject(SKEMAType.ANY);
            }
        }

        public SKEMAObject(Dictionary<string, SKEMAObject> mapSKEMA, bool loose = false)
        {
            this.type = SKEMAType.MAP;

            this.mapSKEMA = mapSKEMA;

            this.loose = loose;
        }

        private SKEMAObject(SKEMAObject arraySKEMA)
        {
            type = SKEMAType.ARRAY;

            this.arraySKEMA = arraySKEMA;
        }

        public SKEMAObject(string reference)
        {
            this.type = SKEMAType.REFERENCE;
            this.reference = reference;
        }

        public SKEMAType Type => type;
        
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

        public bool ResolveReferences(Dictionary<string, SKEMAObject> definitions)
        {
            if ((this.Type == SKEMAType.MAP || this.Type == SKEMAType.ARRAY) == false)
            {
                throw new InvalidOperationException("Can only resolve references for SKEMAObejcts of type MAP and ARRAY!");
            }

            // TODO: Find all strongly connected components in definitions.

            // TODO: substiture all references with their definition.

            throw new NotImplementedException();
        }
        
        public bool Valid(SKONObject obj)
        {
            if (this.type != SKEMAType.ANY && (int)this.type != (int)obj.Type)
            {
                return false;
            }

            switch (this.type)
            {
                case SKEMAType.ANY:
                case SKEMAType.STRING:
                case SKEMAType.INTEGER:
                case SKEMAType.FLOAT:
                case SKEMAType.BOOLEAN:
                case SKEMAType.DATETIME:
                    return true;
                case SKEMAType.MAP:
                    if (loose == false && mapSKEMA.Keys.Count != obj.Keys.Count)
                    {
                        return false;
                    }
                    
                    if (obj.ContainsAllKeys(mapSKEMA.Keys) == false)
                    {
                        return false;
                    }
                    
                    foreach (string key in mapSKEMA.Keys)
                    {
                        if (mapSKEMA[key].Valid(obj[key]) == false)
                        {
                            return false;
                        }
                    }

                    return true;
                case SKEMAType.ARRAY:
                    foreach (SKONObject value in obj.Values)
                    {
                        if (arraySKEMA.Valid(value) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
}