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
        public const int LanguageVersion = 1;

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

            ReferenceSolver rs = new ReferenceSolver();

            // The strongly connected components. Should be used for better error messages.
            List<LinkedList<SKEMAObject>> components;

            if (rs.ResolveReferences(parser.data, parser.definitions, out components) == false)
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
        public static string Write(SKEMAObject obj, SKONMetadata metadata = default(SKONMetadata))
        {
            if (obj.Type != SKEMAType.MAP)
            {
                throw new ArgumentException("SKEMAObject to write must be of type map!");
            }
            
            // We might want to do something here?
            metadata.LanguageVersion = LanguageVersion;

            StringBuilder sb = new StringBuilder();

            sb.Append($"~Version: {metadata.LanguageVersion}~");
            sb.Append($"~DocumentVersion: \"{metadata.DocuemntVersion}\"~");
            if (metadata.SKEMA != null && metadata.SKEMA.Length > 0)
            {
                sb.Append($"~SKEMA: \"{metadata.SKEMA}\"~");
            }

            List<LinkedList<SKEMAObject>> sccs = new ReferenceSolver().FindStronglyConnectedComponents(obj, (v) =>
            {
                List<SKEMAObject> successors = new List<SKEMAObject>();

                switch (v.Type)
                {
                    case SKEMAType.REFERENCE:
                        if (v.ReferenceSKEMA.Type == SKEMAType.REFERENCE || v.ReferenceSKEMA.Type == SKEMAType.ARRAY || v.ReferenceSKEMA.Type == SKEMAType.MAP)
                        {
                            successors.Add(v.ReferenceSKEMA);
                        }
                        break;
                    case SKEMAType.ANY:
                    case SKEMAType.STRING:
                    case SKEMAType.INTEGER:
                    case SKEMAType.FLOAT:
                    case SKEMAType.BOOLEAN:
                    case SKEMAType.DATETIME:
                        break;
                    case SKEMAType.MAP:
                        foreach (string key in v.Keys)
                        {
                            SKEMAObject w = v[key];
                            if (w.Type == SKEMAType.REFERENCE || w.Type == SKEMAType.ARRAY || w.Type == SKEMAType.MAP)
                            {
                                successors.Add(w);
                            }
                        }
                        break;
                    case SKEMAType.ARRAY:
                        if(v.ArrayElementSKEMA.Type == SKEMAType.REFERENCE || v.ArrayElementSKEMA.Type == SKEMAType.ARRAY || v.ArrayElementSKEMA.Type == SKEMAType.MAP)
                        {
                            successors.Add(v.ArrayElementSKEMA);
                        }
                        break;
                    default:
                        break;
                }

                //Console.WriteLine($"Found {successors.Count} successsors!");
                foreach (var s in successors)
                {
                    //Console.WriteLine(s.Type);
                }

                return successors;
            });

            if (sccs.Count > 1)
            {
                List<LinkedList<SKEMAObject>> badSccs = new List<LinkedList<SKEMAObject>>();

                foreach (var scc in sccs)
                {
                    //Console.WriteLine($"Component (Length {scc.Count}):");

                    if (scc.Count == 1 && scc.First.Value.Type != SKEMAType.REFERENCE)
                    {
                        // This strongly connected component can't be bad
                        continue;
                    }

                    bool containsDefiniton = false;
                    
                    foreach (SKEMAObject element in scc)
                    {
                        //Console.WriteLine(element.Type);

                        if (element.Type == SKEMAType.REFERENCE && element.ReferenceSKEMA != null)
                        {
                            containsDefiniton = true;
                            //Console.WriteLine("Component Contains Definition!");
                            break;
                        }
                    }

                    if (containsDefiniton == false)
                    {
                        badSccs.Add(scc);
                    }
                }

                if (badSccs.Count > 0)
                {
                    throw new CouldNotSolveReferencesException(badSccs);
                }
            }
            
            List<SKEMAObject> references = FindReferences(obj);

            foreach (SKEMAObject reference in references)
            {
                sb.Append("define " + reference.Reference + ": " + WriteObject(reference.ReferenceSKEMA, 0) + ",\n");
            }

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
                        mapsb.Append(indentString + SKON.IndentString + (obj.IsOptional(key) ? "optional " : "") + $"{key}: {WriteObject(obj[key], indent + 1)},\n");
                    }

                    mapsb.Append(indentString + "}");

                    return mapsb.ToString();
                case SKEMAType.ARRAY:
                    if (obj.ArrayElementSKEMA == null)
                    {
                        return "[  ]";
                    }
                    return "\n" + 
                        indentString +"[" + ((obj.ArrayElementSKEMA.Type != SKEMAType.MAP && obj.ArrayElementSKEMA.Type != SKEMAType.ARRAY) ? "\n" : "") + 
                        indentString + SKON.IndentString + WriteObject(obj.ArrayElementSKEMA, indent + 1) + "\n" + 
                        indentString + "]";
                default:
                    return null;
            }
        }

        private static List<SKEMAObject> FlattenTree(SKEMAObject skema, bool skipOptional = false)
        {
            // This would be a set if we where targeting .NET greater than 3.5
            List<SKEMAObject> result = new List<SKEMAObject>();
            
            FlattenTreeInternal(result, skema, skipOptional);

            return result;
        }

        private static void FlattenTreeInternal(List<SKEMAObject> result, SKEMAObject skema, bool skipOptional)
        {
            result.Add(skema);

            switch (skema.Type)
            {
                case SKEMAType.REFERENCE:
                    if (skema.ReferenceSKEMA != null)
                    {
                        if (result.Contains(skema.ReferenceSKEMA) == false)
                        {
                            FlattenTreeInternal(result, skema.ReferenceSKEMA, skipOptional);
                        }
                    }
                    break;
                case SKEMAType.ANY:
                case SKEMAType.STRING:
                case SKEMAType.INTEGER:
                case SKEMAType.FLOAT:
                case SKEMAType.BOOLEAN:
                case SKEMAType.DATETIME:
                    break;
                case SKEMAType.MAP:
                    foreach (string key in skema.Keys)
                    {
                        if (skipOptional && skema.IsOptional(key) == true)
                        {
                            //Console.WriteLine("Key " + key + " is optional. Skipping!");
                            continue;
                        }

                        if (result.Contains(skema[key]) == false)
                        {
                            FlattenTreeInternal(result, skema[key], skipOptional);
                        }
                    }
                    break;
                case SKEMAType.ARRAY:
                    if (skema.ArrayElementSKEMA != null)
                    {
                        if (result.Contains(skema.ArrayElementSKEMA) == false)
                        {
                            FlattenTreeInternal(result, skema.ArrayElementSKEMA, skipOptional);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        
        private static List<SKEMAObject> FindReferences(SKEMAObject skema, bool skipOptional = false, bool removeSelf = true)
        {
            List<SKEMAObject> references = new List<SKEMAObject>();
            
            references = FlattenTree(skema, skipOptional).FindAll(s => s.Type == SKEMAType.REFERENCE);

            if (removeSelf && skema.Type == SKEMAType.REFERENCE)
            {
                references.Remove(skema);
            }

            return references;
        }
        
        private class ReferenceSolver
        {
            List<LinkedList<SKEMAObject>> stronglyConnectedComponents = new List<LinkedList<SKEMAObject>>();

            int index = 0;
            Stack<SKEMAObject> S = new Stack<SKEMAObject>();

            Dictionary<SKEMAObject, int> indexMap = new Dictionary<SKEMAObject, int>();
            Dictionary<SKEMAObject, int> lowlinkMap = new Dictionary<SKEMAObject, int>();
            
            internal bool ResolveReferences(SKEMAObject data, Dictionary<string, SKEMAObject> definitions, out List<LinkedList<SKEMAObject>> components)
            {
                //Console.WriteLine($"Resolving references... ({definitions.Count} definitions)");

                // See https://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm for the algorithm used

                components = FindStronglyConnectedComponents(definitions, (v) => 
                {
                    List<SKEMAObject> successors = new List<SKEMAObject>();
                    foreach (var successor in FindReferences(v, true, false))
                    {
                        if (definitions.ContainsKey(successor.Reference))
                        {
                            successors.Add(definitions[successor.Reference]);
                        }
                        else
                        {
                            throw new DefinitionNotFoundException(successor);
                        }
                    }
                    return successors;
                });
                
                if (components.Count > 0)
                {
                    foreach (var comp in components)
                    {
                        if (comp.Count == 1 && comp.First.Value.Type == SKEMAType.REFERENCE)
                        {
                            if (definitions[comp.First.Value.Reference] == comp.First.Value)
                            {
                                return false;
                            }
                        }

                        if (comp.Count > 1)
                        {
                            return false;
                        }
                    }
                }

                //Console.WriteLine("Substituting references...");

                SubstituteDefinitionReferences(definitions);

                SubstituteReferences(data, definitions);

                return true;
            }
            
            internal List<LinkedList<SKEMAObject>> FindStronglyConnectedComponents(SKEMAObject obj, VertexSuccessors VertexSuccessors)
            {
                foreach (string v in obj.Keys)
                {
                    if (indexMap.ContainsKey(obj[v]) == false)
                    {
                        StrongConnect(obj[v], VertexSuccessors);
                    }
                }

                return stronglyConnectedComponents;
            }

            //FIXME: This method is not working as the itterated objects are not the same as the ones in the definitions Dictionary

            private void SubstituteDefinitionReferences(Dictionary<string, SKEMAObject> definitions)
            {
                foreach (string defKey in definitions.Keys)
                {
                    SubstituteReferences(definitions[defKey], definitions);
                }
            }

            private void SubstituteReferences(SKEMAObject obj, Dictionary<string, SKEMAObject> definitions)
            {
                List<SKEMAObject> references = FindReferences(obj);

                foreach (var reference in references)
                {
                    if (reference.ReferenceSKEMA != null)
                    {
                        continue;
                    }

                    //Console.WriteLine($"Replacing reference with definition for {reference.Reference}");
                    if (definitions.ContainsKey(reference.Reference))
                    {
                        reference.ReferenceSKEMA = definitions[reference.Reference];
                        SubstituteReferences(reference.ReferenceSKEMA, definitions);
                    }
                    else
                    {
                        throw new DefinitionNotFoundException(reference);
                    }
                }
            }

            private void StrongConnect(SKEMAObject v, VertexSuccessors VertexSuccessors)
            {
                indexMap[v] = index;
                lowlinkMap[v] = index;
                index++;
                S.Push(v);

                List<SKEMAObject> successors = VertexSuccessors(v);
                
                SKEMAObject w;

                // Find the referenced definitions in the SKEMAObject
                foreach (SKEMAObject reference in successors)
                {
                    w = reference;

                    if (indexMap.ContainsKey(w) == false)
                    {
                        StrongConnect(w, VertexSuccessors);

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

                    //Console.WriteLine("Added strongly connected component of lenght " + component.Count);
                    stronglyConnectedComponents.Add(component);
                }
            }
            
            internal delegate List<SKEMAObject> VertexSuccessors(SKEMAObject obj);
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

        internal CouldNotSolveReferencesException(List<LinkedList<SKEMAObject>> scc) : base($"Could not solve SKEMA references due to {scc.Count} strongly connected component(s)!")
        {
            stronglyConnectedComponents = scc;
        }
    }
}
