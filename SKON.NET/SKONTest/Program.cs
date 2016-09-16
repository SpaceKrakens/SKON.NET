#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SKONTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SKON;

    /// <summary>
    /// Test program for SKON
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="args">The program arguments</param>
        public static void Main(string[] args)
        {
            string defaultPath = "./SKONTest.skon";

            Console.Write("Input file?:");

            string filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();

            SKONObject data = SKON.LoadFile(filePath);

            sw.Stop();

            Console.WriteLine("Successfully parsed file in {0}ms", sw.ElapsedMilliseconds);

            Console.WriteLine(SKON.Write(data));

            Console.ReadKey();
        }
    }
}
