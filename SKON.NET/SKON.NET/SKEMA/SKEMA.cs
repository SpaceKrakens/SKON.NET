﻿#region LICENSE
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

            ReferenceSolver rs = new ReferenceSolver(parser.definitions);

            // The strongly connected components. Should be used for better error messages.
            List<LinkedList<SKEMAObject>> components;

            if (rs.ResolveReferences(parser.data, out components) == false)
            {
                throw new CouldNotSolveReferencesException(components);
            }
            
            return parser.data;
        }

        public static void WriteToFile(string filepath, SKEMAObject obj)
        {
            File.WriteAllText(filepath, Write(obj), Encoding.UTF8);
        }

        /// <summary>
        /// Writes a SKEMAObject.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <returns>A string to write into a file.</returns>
        public static string Write(SKEMAObject obj)
        {
            if (obj.Type != SKEMAType.MAP)
            {
                throw new ArgumentException("SKEMAObject to write must be of type map!");
            }

            StringBuilder sb = new StringBuilder();

            foreach (string key in obj.Keys)
            {
                sb.Append(key + ": " + WriteObject(obj[key], 0) + ",\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Writes a SKEMAObject value.
        /// </summary>
        /// <param name="obj">The object to write.</param>
        /// <param name="indent">The amount of indentation.</param>
        /// <returns>A string to write into a file.</returns>
        private static string WriteObject(SKEMAObject obj, int indent)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            string indentString = string.Empty;

            for (int i = 0; i < indent; i++)
            {
                indentString += SKON.IndentString;
            }
            
            switch (obj.Type)
            {
                case SKEMAType.REFERENCE:
                    return "#" + obj.Reference;
                case SKEMAType.ANY:
                    return "Any";
                case SKEMAType.STRING:
                    return "String";
                case SKEMAType.INTEGER:
                    return "Integer";
                case SKEMAType.FLOAT:
                    return "Float";
                case SKEMAType.BOOLEAN:
                    return "Boolean";
                case SKEMAType.DATETIME:
                    return "DateTime";
                case SKEMAType.MAP:
                    StringBuilder mapsb = new StringBuilder();

                    if (obj.Keys.Count <= 0)
                    {
                        mapsb.Append("{  }");
                        return mapsb.ToString();
                    }

                    mapsb.Append("\n" + indentString + "{\n");

                    foreach (string key in obj.Keys)
                    {
                        mapsb.Append(indentString + SKON.IndentString + $"{key}: {WriteObject(obj[key], indent + 1)},\n");
                    }

                    mapsb.Append(indentString + "}");

                    return mapsb.ToString();
                case SKEMAType.ARRAY:
                    if (obj.ArrayElementSKEMA == null)
                    {
                        return "[  ]";
                    }
                    return "\n[\n" + WriteObject(obj.ArrayElementSKEMA, indent + 1) + "\n]";
                default:
                    return null;
            }
        }

        private class ReferenceSolver
        {
            List<LinkedList<SKEMAObject>> stronglyConnectedComponents = new List<LinkedList<SKEMAObject>>();

            int index = 0;
            Stack<SKEMAObject> S = new Stack<SKEMAObject>();

            Dictionary<SKEMAObject, int> indexMap = new Dictionary<SKEMAObject, int>();
            Dictionary<SKEMAObject, int> lowlinkMap = new Dictionary<SKEMAObject, int>();

            Dictionary<string, SKEMAObject> definitions;

            public ReferenceSolver(Dictionary<string, SKEMAObject> definitions)
            {
                this.definitions = definitions;
            }

            internal bool ResolveReferences(SKEMAObject obj, out List<LinkedList<SKEMAObject>> components)
            {
                Console.WriteLine($"Resolving references... ({definitions.Count} definitions)");
                
                // See https://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm for the algorithm used

                foreach (SKEMAObject v in definitions.Values)
                {
                    if (indexMap.ContainsKey(v) == false)
                    {
                        StrongConnect(v);
                    }
                }

                components = stronglyConnectedComponents;

                if (stronglyConnectedComponents.Count > 0)
                {
                    return false;
                }

                Console.WriteLine("Substituting references...");

                foreach (SKEMAObject definition in definitions.Values)
                {
                    SubstituteReferences(definition);
                }

                SubstituteReferences(obj);

                return true;
            }

            private void SubstituteReferences(SKEMAObject obj)
            {
                List<SKEMAObject> references = FindReferences(obj);

                foreach (var reference in references)
                {
                    Console.WriteLine($"Replacing reference with definition for {reference.Reference}");
                    if (definitions.ContainsKey(reference.Reference))
                    {
                        reference.ReferenceSKEMA = definitions[reference.Reference];
                    }
                    else
                    {
                        throw new DefinitionNotFoundException(reference);
                    }
                }
            }

            private void StrongConnect(SKEMAObject v)
            {
                indexMap[v] = index;
                lowlinkMap[v] = index;
                index++;
                S.Push(v);

                List<SKEMAObject> references = FindReferences(v, true);

                if (references.Count <= 0)
                {
                    return;
                }

                SKEMAObject w;

                // Find the referenced definitions in the SKEMAObject
                foreach (SKEMAObject reference in references)
                {
                    
                    if (definitions.ContainsKey(reference.Reference))
                    {
                        w = definitions[reference.Reference];
                    }
                    else
                    {
                        throw new DefinitionNotFoundException(reference);
                    }

                    if (indexMap.ContainsKey(w) == false)
                    {
                        StrongConnect(w);

                        lowlinkMap[v] = Math.Min(lowlinkMap[v], lowlinkMap[w]);
                    }
                    else if (S.Contains(w))
                    {
                        lowlinkMap[v] = Math.Min(lowlinkMap[v], indexMap[w]);
                    }
                }
                
                if (lowlinkMap[v] == indexMap[v])
                {
                    LinkedList<SKEMAObject> component = new LinkedList<SKEMAObject>();
                    do
                    {
                        w = S.Pop();
                        component.AddLast(w);
                    } while (w != v);

                    Console.WriteLine("Added strongly connected component of lenght " + component.Count);
                    stronglyConnectedComponents.Add(component);
                }
            }

            private List<SKEMAObject> FindReferences(SKEMAObject skema, bool skipOptional = false)
            {
                List<SKEMAObject> references = new List<SKEMAObject>();
                switch (skema.Type)
                {
                    default:
                    case SKEMAType.REFERENCE:
                        references.Add(skema);
                        return references;
                    case SKEMAType.ANY:
                    case SKEMAType.STRING:
                    case SKEMAType.INTEGER:
                    case SKEMAType.FLOAT:
                    case SKEMAType.BOOLEAN:
                    case SKEMAType.DATETIME:
                        return references;
                    case SKEMAType.MAP:
                        foreach (string key in skema.Keys)
                        {
                            if (skipOptional && skema.IsOptional(key) == true)
                            {
                                Console.WriteLine("Key " + key + " is optional. Skipping!");
                                continue;
                            }

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

    /// <summary>
    /// Thrown when the SKEMA parser couldn't find a definition for a given reference.
    /// </summary>
    public class DefinitionNotFoundException : Exception
    {
        public readonly SKEMAObject Reference;

        internal DefinitionNotFoundException(SKEMAObject reference) : base($"Could not find definition for #{reference.Reference}") { Reference = reference; }
    }

    /// <summary>
    /// Thrown when the SKEMA parser could not solve all references when parsing a SKEMA.
    /// </summary>
    public class CouldNotSolveReferencesException : Exception
    {
        /// <summary>
        /// The strongly connected components responsible for the exception.
        /// </summary>
        public List<LinkedList<SKEMAObject>> stronglyConnectedComponents;

        internal CouldNotSolveReferencesException(List<LinkedList<SKEMAObject>> scc) : base($"Could not solve SKEMA references due to {scc.Count} strongly connected components!")
        {
            stronglyConnectedComponents = scc;
        }
    }
}
