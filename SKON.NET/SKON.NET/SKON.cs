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

    using Internal;

    /// <summary>
    /// Central class for all SKON related functions.
    /// </summary> 
    public static class SKON
    {
        /// <summary>
        /// The indent spaces.
        /// </summary>
        private const string IndentSpaces = "    "; // 4 spaces

        /// <summary>
        /// The indent tab.
        /// </summary>
        private const string IndentTab = "\t"; // A tab

        /// <summary>
        /// Gets or sets a value indicating whether to use tabs.
        /// </summary>
        private static bool UseTabs { get; set; }

        /// <summary>
        /// The indent string.
        /// </summary>
        private static string IndentString => UseTabs ? IndentTab : IndentSpaces;

        /// <summary>
        /// Loads a text file as a SKON Map.
        /// </summary>
        /// <param name="path">Full FilePath to the SKON text file.</param>
        /// <param name="errorStream">The TextWriter to write error messages to.</param>
        /// <returns>The root map containing all SKONObjects.</returns>
        public static SKONObject LoadFile(string path, TextWriter errorStream = null)
        {
            if (File.Exists(path))
            {
                Scanner sc = new Scanner(path);
                Parser parser = new Parser(sc);

                if (errorStream != null)
                {
                    parser.errors.errorStream = errorStream;
                }
                
                parser.Parse();

                return parser.data;
            }
            else
            {
                throw new FileNotFoundException("File not found!", path);
            }
        }

        /// <summary>
        /// Parses a SKON string to a SKONObject.
        /// </summary>
        /// <param name="skon">The SKON data string.</param>
        /// <param name="errorStream">The TextWriter to write error messages to.</param>
        /// <returns>The newly created SKONObject.</returns>
        public static SKONObject Parse(string skon, TextWriter errorStream = null)
        {
            Scanner sc = new Scanner(GenerateStreamFromString(skon));
            Parser parser = new Parser(sc);

            if (errorStream != null)
            {
                parser.errors.errorStream = errorStream;
            }
            
            parser.Parse();

            return parser.data;
        }

        /// <summary>
        /// Writes a SKONObject to a file. This will overwrite the current content of the file.
        /// </summary>
        /// <param name="filepath">The path to the file to write to.</param>
        /// <param name="obj">The SKONObject to turn into a string and write to a file.</param>
        public static void WriteToFile(string filepath, SKONObject obj)
        {
            if (obj.Type != ValueType.MAP)
            {
                throw new ArgumentException("SKONObject to write must be of type map!");
            }

            File.WriteAllText(filepath, Write(obj), Encoding.UTF8);
        }

        /// <summary>
        /// Writes a SKONObject.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <returns>A string to write into a file.</returns>
        public static string Write(SKONObject obj)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in obj.Keys)
            {
                sb.Append(key + ": " + WriteObject(obj[key], 0) + ",\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates a UTF8 stream from a string.
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>The UTF8 stream</returns>
        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? string.Empty));
        }

        /// <summary>
        /// Writes a SKONObject value.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <param name="indent">The amount of indentation.</param>
        /// <returns>A string to write into a file.</returns>
        private static string WriteObject(SKONObject obj, int indent)
        {
            string indentString = string.Empty;

            for (int i = 0; i < indent; i++)
            {
                indentString += IndentString;
            }

            switch (obj.Type)
            {
                case ValueType.EMPTY:
                    return "null";
                case ValueType.STRING:
                    return "\"" + obj.String + "\"";
                case ValueType.INTEGER:
                    return obj.Int.ToString();
                case ValueType.DOUBLE:
                    return obj.Double.ToString().Replace(',', '.');
                case ValueType.BOOLEAN:
                    return obj.Boolean.ToString().ToLower();
                case ValueType.DATETIME:
                    return (obj.DateTime ?? default(DateTime)).ToString("yyyy-MM-ddThh:mm:ss.fffzzz");
                case ValueType.MAP:
                    StringBuilder mapsb = new StringBuilder();

                    if (obj.Keys.Count <= 0)
                    {
                        mapsb.Append("{  }");
                        return mapsb.ToString();
                    }

                    mapsb.Append('\n' + indentString + "{\n");

                    foreach (string key in obj.Keys)
                    {
                        mapsb.Append(indentString + IndentString + $"{key}: {WriteObject(obj[key], indent + 1)},\n");
                    }

                    mapsb.Append(indentString + "}");

                    return mapsb.ToString();
                case ValueType.ARRAY:
                    StringBuilder arraysb = new StringBuilder();

                    if (obj.Length <= 0)
                    {
                        arraysb.Append("[  ]");
                        return arraysb.ToString();
                    }

                    arraysb.Append('\n' + indentString + "[\n");

                    for (int i = 0; i < obj.Length; i++)
                    {
                        arraysb.Append(indentString + IndentString + $"{WriteObject(obj[i], indent + 1)},\n");
                    }

                    arraysb.Append(indentString + "]");

                    return arraysb.ToString();
                default:
                    return null;
            }
        }
    }
}
