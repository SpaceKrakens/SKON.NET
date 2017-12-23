#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SKON.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SKON
{
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.Collections.Generic;

    using Internal;
    using Internal.Utils;
    using System.Globalization;

    public struct SKONMetadata
    {
        public int LanguageVersion { get; internal set; }
        public string DocuemntVersion { get; internal set; }
        public string SKEMA { get; internal set; }
    }

    /// <summary>
    /// Central class for all SKON related functions.
    /// </summary> 
    public static class SKON
    {
        public const int LanguageVersion = 1;

        internal const char Metadelimit = '-';

        /// <summary>
        /// The indent spaces.
        /// </summary>
        private const string IndentSpaces = "    ";

        /// <summary>
        /// The indent tab.
        /// </summary>
        private const string IndentTab = "\t";

        /// <summary>
        /// Gets or sets a value indicating whether to use tabs.
        /// </summary>
        public static bool UseTabs { get; set; }

        /// <summary>
        /// The indent string.
        /// </summary>
        internal static string IndentString => UseTabs ? IndentTab : IndentSpaces;

        public static SKONObject LoadFile(string path, TextWriter errorStream = null)
        {
            SKONMetadata metadata;
            return LoadFile(path, out metadata, errorStream);
        }

        /// <summary>
        /// Loads a text file as a SKON Map.
        /// </summary>
        /// <param name="path">Full FilePath to the SKON text file.</param>
        /// <param name="errorStream">The TextWriter to write error messages to.</param>
        /// <returns>The root map containing all SKONObjects.</returns>
        public static SKONObject LoadFile(string path, out SKONMetadata metadata, TextWriter errorStream = null)
        {
            if (File.Exists(path))
            {
                Scanner sc = new Scanner(path);

                return Parse(sc, out metadata, errorStream);
            }
            else
            {
                throw new FileNotFoundException("File not found!", path);
            }
        }

        public static SKONObject Parse(string skon, TextWriter errorStream = null)
        {
            SKONMetadata metadata;
            return Parse(skon, out metadata, errorStream);
        }

        /// <summary>
        /// Parses a SKON string to a SKONObject.
        /// </summary>
        /// <param name="skon">The SKON data string.</param>
        /// <param name="errorStream">The TextWriter to write error messages to.</param>
        /// <returns>The newly created SKONObject.</returns>
        public static SKONObject Parse(string skon, out SKONMetadata metadata, TextWriter errorStream = null)
        {
            using (MemoryStream stream = ParserUtils.GenerateStreamFromString(skon))
            {
                Scanner sc = new Scanner(stream);
                
                return Parse(sc, out metadata, errorStream);
            }
        }

        public static SKONObject Parse(Stream stream, TextWriter errorStream = null)
        {
            SKONMetadata metadata;
            return Parse(stream, out metadata, errorStream);
        }

        /// <summary>
        /// Parses a SKON stream to a SKONObject.
        /// </summary>
        /// <param name="stream">The SKON data stream.</param>
        /// <param name="errorStream">The TextWriter to write error messages to.</param>
        /// <returns></returns>
        public static SKONObject Parse(Stream stream, out SKONMetadata metadata, TextWriter errorStream = null)
        {
            Scanner sc = new Scanner(stream);

            return Parse(sc, out metadata, errorStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="errorStream"></param>
        /// <returns></returns>
        private static SKONObject Parse(Scanner sc, out SKONMetadata metadata, TextWriter errorStream)
        {
            Parser parser = new Parser(sc);

            if (errorStream != null)
            {
                parser.errors.errorStream = errorStream;
            }
            
            parser.Parse();

            if (parser.errors.count > 0)
            {
                throw new FormatException(string.Format("Could not parse file! Got {0} errors!", parser.errors.count));
            }

            metadata = parser.metadata;

            return parser.data;
        }
        
        /// <summary>
        /// Writes a SKONObject to a file. This will overwrite the current content of the file.
        /// </summary>
        /// <param name="filepath">The path to the file to write to.</param>
        /// <param name="obj">The SKONObject to turn into a string and write to a file.</param>
        public static void WriteToFile(string filepath, SKONObject obj)
        {
            File.WriteAllText(filepath, Write(obj), Encoding.UTF8);
        }

        /// <summary>
        /// Writes a SKONObject.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <returns>A string to write into a file.</returns>
        public static string Write(SKONObject obj, SKONMetadata metadata = default(SKONMetadata))
        {
            if (obj.Type != SKONValueType.MAP)
            {
                throw new ArgumentException("SKONObject to write must be of type map!");
            }

            if (ContainsLoops(obj))
            {
                throw new ArgumentException("Could not write SKONObject due to recursive references!");
            }

            //There might be something we would want to do if the versions don't match
            metadata.LanguageVersion = LanguageVersion;

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Metadelimit}Version: {metadata.LanguageVersion}{Metadelimit}\n");
            sb.Append($"{Metadelimit}DocumentVersion: \"{metadata.DocuemntVersion}\"{Metadelimit}\n");
            if (metadata.SKEMA != null && metadata.SKEMA.Length > 0)
            {
                sb.Append($"{Metadelimit}SKEMA: {metadata.SKEMA}{Metadelimit}\n");
            }

            sb.Append('\n');

            foreach (string key in obj.Keys)
            {
                sb.Append(key + ": ");

                sb.Append(obj[key].IsComplex ? '\n' : ' ');

                sb.Append(WriteObject(obj[key], 0));
            }

            return sb.ToString();
        }
        
        public static bool ContainsLoops(SKONObject obj)
        {
            // This should be a HashMap but .NET 2.0 does not have this data type
            Dictionary<SKONObject, bool> contains = new Dictionary<SKONObject, bool>();

            return ContainsLoopsInternal(obj, contains);
        }

        private static bool ContainsLoopsInternal(SKONObject obj, Dictionary<SKONObject, bool> contains)
        {
            if (contains.ContainsKey(obj))
            {
                return true;
            }

            contains.Add(obj, true);

            switch (obj.Type)
            {
                case SKONValueType.EMPTY:
                case SKONValueType.STRING:
                case SKONValueType.INTEGER:
                case SKONValueType.FLOAT:
                case SKONValueType.BOOLEAN:
                case SKONValueType.DATETIME:
                    return false;
                case SKONValueType.MAP:
                    foreach (var key in obj.Keys)
                    {
                        if (ContainsLoopsInternal(obj[key], new Dictionary<SKONObject, bool>(contains)))
                        {
                            return true;
                        }
                    }

                    return false;
                case SKONValueType.ARRAY:
                    foreach (var element in obj.Values)
                    {
                        if (ContainsLoopsInternal(element, new Dictionary<SKONObject, bool>(contains)))
                        {
                            return true;
                        }
                    }

                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Writes a SKONObject value.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <param name="indent">The amount of indentation.</param>
        /// <returns>A string to write into a file.</returns>
        private static string WriteObject(SKONObject obj, int indent)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            string indentString = string.Empty;

            for (int i = 0; i < indent; i++)
            {
                indentString += IndentString;
            }

            switch (obj.Type)
            {
                case SKONValueType.EMPTY:
                    return indentString + "null,\n";
                case SKONValueType.STRING:
                    return indentString + "\"" + obj.String + "\",\n";
                case SKONValueType.INTEGER:
                    return indentString + obj.Int.ToString() + ",\n";
                case SKONValueType.FLOAT:
                    return indentString + obj.Double.Value.ToString(CultureInfo.InvariantCulture) + ",\n";
                case SKONValueType.BOOLEAN:
                    return indentString + obj.Boolean.ToString().ToLower() + ",\n";
                case SKONValueType.DATETIME:
                    return indentString + (obj.DateTime ?? default(DateTime)).ToString("yyyy-MM-ddThh:mm:ss.fffzzz") + ",\n";
                case SKONValueType.MAP:
                    StringBuilder mapsb = new StringBuilder();

                    mapsb.Append(indentString);

                    if (obj.Keys.Count <= 0)
                    {
                        mapsb.Append("{  },\n");
                        return mapsb.ToString();
                    }

                    mapsb.Append("{\n");

                    foreach (string key in obj.Keys)
                    {
                        mapsb.Append(indentString).Append(IndentString);
                        mapsb.Append(key).Append(":");

                        mapsb.Append(obj[key].IsComplex ? '\n' : ' ');

                        mapsb.Append(WriteObject(obj[key], obj[key].IsComplex ? indent + 1 : 0));
                    }

                    mapsb.Append(indentString).Append("},\n");

                    return mapsb.ToString();
                case SKONValueType.ARRAY:
                    StringBuilder arraysb = new StringBuilder();

                    if (obj.Length <= 0)
                    {
                        arraysb.Append("[  ]\n");
                        return arraysb.ToString();
                    }

                    arraysb.Append(indentString + "[\n");

                    for (int i = 0; i < obj.Length; i++)
                    {
                        arraysb.Append($"{WriteObject(obj[i], indent + 1)}");
                    }

                    arraysb.Append(indentString).Append("],\n");

                    return arraysb.ToString();
                default:
                    return "!!internal error!!";
            }
        }
    }
}
