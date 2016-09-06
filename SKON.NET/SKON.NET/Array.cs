using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKON.NET
{
    public class Array : SKONObject
    {
        private SKONObject[] arrayValues;

        internal Array(SKONObject[] arrayValues)
        {
            this.arrayValues = arrayValues;
        }

        public override SKONObject this[int i]
        {
            get
            {
                return arrayValues[i];
            }
        }
    }
}
