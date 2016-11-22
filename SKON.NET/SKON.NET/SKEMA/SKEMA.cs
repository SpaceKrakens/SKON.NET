#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SKEMA.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SKON.SKEMA
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Internal;
    using global::SKON.Internal.Utils;

    public class SKEMA
    {
        public static SKEMAObject LoadFile(string path, TextWriter errorStream = null)
        {
            if (File.Exists(path))
            {
                Scanner sc = new Scanner(path);

                return Parse(sc, errorStream);
            }
            else
            {
                throw new FileNotFoundException("File not found!", path);
            }
        }

        public static SKEMAObject Parse(string skon, TextWriter errorStream = null)
        {
            using (MemoryStream stream = ParserUtils.GenerateStreamFromString(skon))
            {
                Scanner sc = new Scanner(stream);

                return Parse(sc, errorStream);
            }
        }

        public static SKEMAObject Parse(Stream stream, TextWriter errorStream = null)
        {
            Scanner sc = new Scanner(stream);

            return Parse(sc, errorStream);
        }

        private static SKEMAObject Parse(Scanner sc, TextWriter errorStream)
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

            new ReferenceSolver(parser.definitions).ResolveReferences();
            
            return parser.data;
        }

        public class ReferenceSolver
        {
            int index = 0;
            Stack<SKEMAObject> S = new Stack<SKEMAObject>();

            Dictionary<SKEMAObject, int> indexMap = new Dictionary<SKEMAObject, int>();
            Dictionary<SKEMAObject, int> lowlinkMap = new Dictionary<SKEMAObject, int>();

            Dictionary<string, SKEMAObject> definitions;

            public ReferenceSolver(Dictionary<string, SKEMAObject> definitions)
            {
                this.definitions = definitions;
            }

            internal bool ResolveReferences()
            {
                // TODO: Find all strongly connected components in definitions.
                foreach (var v in definitions.Values)
                {
                    if (indexMap.ContainsKey(v))
                    {
                        StrongConnect(v);
                    }
                }

                // TODO: substiture all references with their definition.

                throw new NotImplementedException();
            }

            private LinkedList<SKEMAObject> StrongConnect(SKEMAObject v)
            {
                indexMap[v] = index;
                lowlinkMap[v] = index;
                index++;
                S.Push(v);

                // Find the referenced definitions in the SKEMAObject



                return null;
            }

            private List<SKEMAObject> FindReferences(SKEMAObject skema)
            {
                List<SKEMAObject> references = new List<SKEMAObject>();
                switch (skema.Type)
                {
                    default:
                    case SKEMAType.REFERENCE:
                    case SKEMAType.ANY:
                    case SKEMAType.STRING:
                    case SKEMAType.INTEGER:
                    case SKEMAType.FLOAT:
                    case SKEMAType.BOOLEAN:
                    case SKEMAType.DATETIME:
                        return new List<SKEMAObject>();
                    case SKEMAType.MAP:
                        foreach (string key in skema.Keys)
                        {
                            if (skema[key].Type == SKEMAType.REFERENCE)
                            {
                                references.Add(skema[key]);
                            }
                            else
                            {
                                references.AddRange(FindReferences(skema[key]));
                            }
                        }
                        return references;
                    case SKEMAType.ARRAY:
                        references.AddRange(FindReferences(skema.ArrayElementSKEMA));
                        return references;
                }
            }
        }
    }    
}
