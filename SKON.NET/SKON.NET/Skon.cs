//-----------------------------------------------------------------------
// <copyright file="Skon.cs" company="SpaceKraken">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SKON.NET
{
    /// <summary>
    /// TODO: Add documentation.
    /// </summary>
    public class Skon
    {
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
