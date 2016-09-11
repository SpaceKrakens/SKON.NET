//-----------------------------------------------------------------------
// <copyright file="Skon.cs" company="SpaceKrakens">
//     MIT Licence
//     Copyright (C) 2016 SpaceKrakens
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using SKON.Internal;

namespace SKON
{
    /// <summary>
    /// TODO: Add documentation.
    /// </summary>
    public class Skon
    {
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
