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
        
    }
    
    internal abstract class SKEMASpecifier
    {
        internal class EmptySpecifier : SKEMASpecifier
        {
            public override string GetSKEMAString()
            {
                return string.Empty;
            }

            public override bool Match(SKONObject obj)
            {
                return true;
            }
        }

        public static SKEMASpecifier Empty => new EmptySpecifier();

        public abstract bool Match(SKONObject obj);

        public abstract string GetSKEMAString();
    }
}
