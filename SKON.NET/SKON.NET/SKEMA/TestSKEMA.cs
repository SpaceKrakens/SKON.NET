#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSKON.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// <summary>
//   Defines the TestSKON type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SKON.SKEMA
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    class TestSKEMA
    {
        public static SKEMAObject TestSKEMAObject => new Dictionary<string, SKEMAObject>
        {
            { "Version", SKEMAObject.String },
            { "VersionName", SKEMAObject.String },
            { "Author", new Dictionary<string, SKEMAObject>
                {
                    { "FistName", SKEMAObject.String },
                    { "LastName", SKEMAObject.String },
                    { "Nickname", SKEMAObject.String },
                    { "Email", SKEMAObject.String }
                }
            },
            { "ExampleString", SKEMAObject.String },
            { "ExampleInteger", SKEMAObject.Integer },
            { "ExampleFloat", SKEMAObject.Float },
            { "ExampleBoolean", SKEMAObject.Boolean },
            { "ExampleDateTime", SKEMAObject.DateTime },
            { "ExampleArray", SKEMAObject.ArrayOf(SKEMAObject.String) },
            { "ExampleMap", new Dictionary<string, SKEMAObject>
                {
                    { "ThisIsAKey", SKEMAObject.String },
                    { "ThisIsAnotherKey", SKEMAObject.Integer }
                }
            },
            { "ArrayOfMaps", SKEMAObject.ArrayOf(new Dictionary<string, SKEMAObject>
                {
                    { "Key", SKEMAObject.String }
                })
            },
            { "MapOfAllDataTypes", new Dictionary<string, SKEMAObject>
                {
                    { "String", SKEMAObject.String },
                    { "Integer", SKEMAObject.Integer },
                    { "Float", SKEMAObject.Float },
                    { "Boolean", SKEMAObject.Boolean },
                    { "DateTime", SKEMAObject.DateTime },
                    { "Array", SKEMAObject.ArrayOf(SKEMAObject.Any) },
                    { "Map", new Dictionary<string, SKEMAObject>() }
                }
            },
            { "ArrayOfArrayOfStrings", SKEMAObject.ArrayOf(SKEMAObject.ArrayOf(SKEMAObject.String)) }
        };
    }
}
