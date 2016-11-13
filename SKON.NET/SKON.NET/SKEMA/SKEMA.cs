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
                throw new FormatException(string.Format("Could not parse file! Got {0} errors!"));
            }

            parser.data.ResolveReferences(parser.definitions);

            return parser.data;
        }
    }
}
