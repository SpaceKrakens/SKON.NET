//-----------------------------------------------------------------------
// <copyright file="SKON.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

namespace SKON
{
    using System.IO;

    using Internal;

    /// <summary>
    /// TODO: Add documentation.
    /// </summary>
    public class SKON
    {
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
    }
}
