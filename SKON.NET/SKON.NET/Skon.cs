//-----------------------------------------------------------------------
// <copyright file="SKON.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using SKON.Internal;

namespace SKON
{
    using System.IO;

    using Internal;

    /// <summary>
    /// TODO: Add documentation.
    /// </summary>
    public class SKON
    {

        private static readonly string IndentSpaces = "    "; // 4 spaces

        private static readonly string IndentTab = "\t"; // A tab

        public static bool UseTabs { get; set; }

        /// <summary>
        /// Loads a text file as a SKON Map.
        /// </summary>
        /// <param name="path">Full FilePath to the SKON text file.</param>
        /// <returns>The root map containing all SKONObjects.</returns>
        public static SKONObject LoadFile(string path)
        {
            if (File.Exists(path))
            {
                Scanner sc = new Scanner(path);
                Parser parser = new Parser(sc);

                parser.Parse();

                return parser.data;
            }
            else
            {
                throw new FileNotFoundException("File not found!", path);
            }
        }
        
        public static string Write(SKONObject obj)
        {
            return WriteObject(obj, 0);
        }
        
        private static string WriteObject(SKONObject obj, int indent)
        {
            string indentString = string.Empty;

            for (int i = 0; i < indent; i++)
            {
                indentString += UseTabs ? IndentTab : IndentSpaces;
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
                    return obj.Double.ToString();
                case ValueType.BOOLEAN:
                    return obj.Boolean.ToString();
                case ValueType.DATETIME:
                    return obj.DateTime.ToString();
                case ValueType.MAP:
                    StringBuilder mapsb = new StringBuilder();

                    mapsb.Append(indentString + "{\n");

                    foreach (var key in obj.Keys)
                    {
                        mapsb.Append(indentString + $"{key}: { WriteObject(obj[key], indent) },\n");
                    }

                    mapsb.Append(indentString + "}");

                    return mapsb.ToString();
                case ValueType.ARRAY:
                    StringBuilder arraysb = new StringBuilder();

                    arraysb.Append(indentString + "[\n");

                    for (int i = 0; i < obj.Length; i++)
                    {
                        arraysb.Append(indentString + $"{ WriteObject(obj[i], indent + 1) },\n");
                    }

                    arraysb.Append(indentString + "]\n");

                    return arraysb.ToString();
                default:
                    return null;
            }
        }
    }
}
