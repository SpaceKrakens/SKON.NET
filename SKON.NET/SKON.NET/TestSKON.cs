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

namespace SKON
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// The test SKON.
    /// </summary>
    public static class TestSKON
    {
        /// <summary>
        /// Gets the official test SKON object.
        /// </summary>
        public static SKONObject TestSKONObject => new Dictionary<string, SKONObject>
        {
            {
                "Version", "0.0.1"
            },
            {
                "VersionName", "Octopus"
            },
            {
                "Author", new Dictionary<string, SKONObject>
                {
                    {
                        "FirstName", "Julius"
                    },
                    {
                        "LastName", "Häger"
                    },
                    {
                        "Nickname", "Noggin_Bops"
                    },
                    {
                        "Email", "Julius_hager@hotmail.com"
                    }
                }
            },
            {
                "ExampleString", "This is a string"
            },
            {
                "ExampleInteger", 10
            },
            {
                "ExampleFloat", 0.4
            },
            {
                "ExampleBoolean", true
            },
            {
                "ExampleDateTime", Internal.Utils.ParserUtils.UnixTimeStampToDateTime(296638320L)
            },
            {
                "ExampleArray", new[]
                {
                    "This",
                    "is",
                    "an",
                    "array",
                    "of",
                    "strings"
                }
            },
            {
                "ExampleMap", new Dictionary<string, SKONObject>
                {
                    {
                        "ThisIsAKey", "To a string value"
                    },
                    {
                        "ThisIsAnotherKey", 1
                    }
                }
            },
            {
                "ArrayOfMaps", new List<SKONObject>
                {
                    new Dictionary<string, SKONObject>
                        {
                            {
                                "Key", "Value"
                            }
                        },
                    new Dictionary<string, SKONObject>
                        {
                            {
                                "Key", "AnotherValue"
                            }
                        },
                    new Dictionary<string, SKONObject>
                        {
                            {
                                "Key", "YetAnotherValue"
                            }
                        }
                }
            },
            {
                "MapOfAllDataTypes", new Dictionary<string, SKONObject>
                {
                    {
                        "String", "String type"
                    },
                    {
                        "Integer", 1
                    },
                    {
                        "Float", 0.3
                    },
                    {
                        "Boolean", true
                    },
                    {
                        "DateTime", new DateTime(2016, 09, 11)
                    },
                    {
                        "Array", new List<SKONObject>()
                    },
                    {
                        "Map", new Dictionary<string, SKONObject>()
                    }
                }
            },
            {
                "ArrayOfArrayOfStrings", new List<SKONObject>
                {
                    new[]
                    {
                        "Index: 0_0",
                        "Index: 0_1"
                    },
                    new[]
                    {
                        "Index: 1_0",
                        "Index: 1_1"
                    },
                    new[]
                    {
                        "Index: 2_0",
                        "Index: 2_1"
                    }
                }
            }
        };
    }
}
