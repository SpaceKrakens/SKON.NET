//-----------------------------------------------------------------------
// <copyright file="Map.cs" company="SpaceKrakens">
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
    /// A SKON Map. Holds all kinds of values, accessible with string keys.
    /// </summary>
    public class Map : SKONObject
    {
        /// <summary>
        /// Backing dictionary holding the key-value pairs of a SKON Map.
        /// </summary>
        private Dictionary<string, SKONObject> mapValues;

        /// <summary>
        /// Initialises a new instance of the <see cref="Map"/> class.
        /// Implements a SKON Map as a dictionary of string keys and SKONObject values.
        /// </summary>
        /// <param name="mapValues">A dictionary containing key-value pairs from a SKON Map</param>
        internal Map(Dictionary<string, SKONObject> mapValues)
        {
            this.mapValues = mapValues;
        }

        /// <summary>
        /// Returns the value given inside this Map for the specified key.
        /// </summary>
        /// <param name="key">The key to get the value for.</param>
        /// <returns>The wanted value or null if the key does not exist in this Map.</returns>
        public override SKONObject this[string key]
        {
            get
            {
                SKONObject value;
                if (this.mapValues.TryGetValue(key, out value))
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
