using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKON.NET
{
    public class Map : SKONObject
    {
        private Dictionary<string, SKONObject> mapValues;

        internal Map(Dictionary<string, SKONObject> mapValues)
        {
            this.mapValues = mapValues;
        }

        public override SKONObject this[string key]
        {
            get
            {
                SKONObject value;
                if (mapValues.TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
