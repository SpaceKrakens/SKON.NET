//-----------------------------------------------------------------------
// <copyright file="Skon.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SKON.NET
{
    /// <summary>
    /// TODO: Add documentation.
    /// </summary>
    public class Skon
    {
        /// <summary>
        /// Loads a text file as a SKON Map.
        /// </summary>
        /// <param name="path">Full FilePath to the SKON text file.</param>
        /// <returns>The root map containing all SKONObjects.</returns>
        public static Map LoadFile(string path)
        {
            if (File.Exists(path))
            {
                Scanner sc = new Scanner(path);
                Parser parser = new Parser(sc);

                parser.Parse();

                return parser.map;
            }
            else
            {
                throw new FileNotFoundException("File not found!", path);
            }
        }
    }
}
