//-----------------------------------------------------------------------
// <copyright file="SKONObject.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

namespace SKON.SKEMA
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using ValueType = ValueType;
    
    class SKEMAObject
    {
        private ValueType type;
        
        private Dictionary<string, SKEMAObject> complexValue;

        private SKEMAObject arraySKEMA;

        public bool Validate(SKONObject obj)
        {
            if (this.type != obj.Type)
            {
                return false;
            }

            switch (this.type)
            {
                case ValueType.EMPTY:
                case ValueType.STRING:
                case ValueType.INTEGER:
                case ValueType.DOUBLE:
                case ValueType.BOOLEAN:
                case ValueType.DATETIME:
                    return true;
                case ValueType.MAP:
                    return false;
                case ValueType.ARRAY:
                    foreach (SKONObject value in obj.Values)
                    {
                        if (arraySKEMA.Validate(value) == false)
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
