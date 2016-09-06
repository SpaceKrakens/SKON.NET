using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKON.NET
{
    public class SKONObject
    {
        private string stringValue;
        private int? intValue;
        private float? floatValue;
        private bool? booleanValue;
        private DateTime? dateTimeValue;

        internal SKONObject()
        {

        }

        internal SKONObject(string stringValue)
        {
            this.stringValue = stringValue;
        }

        internal SKONObject(int intValue)
        {
            this.intValue = intValue;
        }

        internal SKONObject(float floatValue)
        {
            this.floatValue = floatValue;
        }

        internal SKONObject(bool booleanValue)
        {
            this.booleanValue = booleanValue;
        }

        internal SKONObject(DateTime dateTimeValue)
        {
            this.dateTimeValue = dateTimeValue;
        }

        public virtual SKONObject this[int i]
        {
            get
            {
                return null;
            }
        }

        public virtual SKONObject this[string key]
        {
            get
            {
                return null;
            }
        }

        public static explicit operator string(SKONObject obj)
        {
            return obj.stringValue;
        }

        public static explicit operator int?(SKONObject obj)
        {
            return obj.intValue;
        }

        public static explicit operator float?(SKONObject obj)
        {
            return obj.floatValue;
        }

        public static explicit operator bool?(SKONObject obj)
        {
            return obj.booleanValue;
        }

        public static explicit operator DateTime?(SKONObject obj)
        {
            return obj.dateTimeValue;
        }
    }
}
