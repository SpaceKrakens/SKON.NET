// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSKON.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
// <summary>
//   Defines the TestSKON type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace SKON
{
    /// <summary>
    /// The test SKON.
    /// </summary>
    public static class TestSKON
    {
        /// <summary>
        /// Gets the official test SKON object.
        /// </summary>
        public static SKONObject TestSKONObject => new SKONObject(new Dictionary<string, SKONObject>
            {
                {
                    "Version", "0.0.1"
                },
                {
                    "VersionName", "Octopus"
                },
                {
                    "Author", new SKONObject(new Dictionary<string, SKONObject>
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
                    })
                },
                {
                    "ExampleString", "This is a string"
                },
                {
                    "ExampleInteger", 10
                },
                {
                    "ExampleDouble", 0.4
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
                    "ExampleMap", new SKONObject(new Dictionary<string, SKONObject>
                    {
                        {
                            "ThisIsAKey", "To a string value"
                        },
                        {
                            "ThisIsAnotherKey", 1
                        }
                    })
                },
                {
                    "ArrayOfMaps", new SKONObject(new List<SKONObject>
                    {
                        new SKONObject(new Dictionary<string, SKONObject>
                            {
                                {
                                    "Key", "Value"
                                }
                            }),
                        new SKONObject(new Dictionary<string, SKONObject>
                            {
                                {
                                    "Key", "AnotherValue"
                                }
                            }),
                        new SKONObject(new Dictionary<string, SKONObject>
                            {
                                {
                                    "Key", "YetAnotherValue"
                                }
                            })
                    })
                },
                {
                    "MapOfAllDataTypes", new SKONObject(new Dictionary<string, SKONObject>
                    {
                        {
                            "String", "String type"
                        },
                        {
                            "Integer", 1
                        },
                        {
                            "Double", 0.3
                        },
                        {
                            "Boolean", true
                        },
                        {
                            "DateTime", new SKONObject(new DateTime(2016, 09, 11))
                        },
                        {
                            "Array", new SKONObject(new List<SKONObject>())
                        },
                        {
                            "Map", new SKONObject(new Dictionary<string, SKONObject>())
                        }
                    })
                },
                {
                    "ArrayOfArrayOfStrings", new SKONObject(new List<SKONObject>
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
                    })
                }
            });
    }
}
