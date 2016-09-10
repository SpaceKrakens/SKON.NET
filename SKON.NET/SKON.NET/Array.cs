//-----------------------------------------------------------------------
// <copyright file="Array.cs" company="SpaceKrakens">
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
    /// A SKON Array. Holds values of one SKON data type.
    /// </summary>
    public class Array : SKONObject
    {
        /// <summary>
        /// Backing Array of SKONObjects.
        /// </summary>
        private SKONObject[] arrayValues;

        /// <summary>
        /// Initialises a new instance of the <see cref="Array"/> class.
        /// Implements a SKON Array as an array of SKONObjects.
        /// </summary>
        /// <param name="arrayValues">An array of SKONObjects that are values for a SKON Array.</param>
        internal Array(SKONObject[] arrayValues)
        {
            this.arrayValues = arrayValues;
        }

        public int Length { get { return arrayValues.Length; } }

        /// <summary>
        /// Returns the value of the SKON Array found at the given index.
        /// </summary>
        /// <param name="i">The index to search for.</param>
        /// <returns>The SKONObject value at the given index or null if the index was out of bounds.</returns>
        public override SKONObject this[int i]
        {
            get
            {
                return i >= 0 && i < this.arrayValues.Length ? this.arrayValues[i] : null;
            }
        }
    }
}
