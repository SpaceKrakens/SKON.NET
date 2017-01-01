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
    using SKON.SKEMA;

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

            Console.WriteLine("Successfully parsed SKON file in {0}ms!", sw.ElapsedMilliseconds);

            Console.WriteLine();
            
            sw.Reset();

            sw.Start();
            SKON.WriteToFile("./ResultSKON.skon", data);
            sw.Stop();

            Console.WriteLine("Successfully wrote file in {0}ms!", sw.ElapsedMilliseconds);

            Console.WriteLine();

            Console.Write("Show written file? Y/N (N):");

            if (Console.ReadLine() == "Y")
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                string[] result = File.ReadAllLines("./ResultSKON.skon");

                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine(result[i]);
                }

                Console.ForegroundColor = color;
            }
            
            defaultPath = "./SKEMATest.skema";

            Console.Write("SKEMA input file?:");

            filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            sw.Reset();

            sw.Start();
            SKEMAObject skema = SKEMA.LoadFile(filePath);
            sw.Stop();

            Console.WriteLine("Successfully parsed SKEMA file in {0}ms!", sw.ElapsedMilliseconds);

            Console.WriteLine();

            sw.Reset();

            sw.Start();
            SKEMA.WriteToFile("./ResultSKEMA.skema", skema);
            sw.Stop();

            Console.WriteLine("Successfully wrote SKEMA file in {0}ms!", sw.ElapsedMilliseconds);

            Console.WriteLine();

            Console.Write("Show written SKEMA file? Y/N (N):");

            if (Console.ReadLine() == "Y")
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                string[] result = File.ReadAllLines("./ResultSKEMA.skema");

                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine(result[i]);
                }

                Console.ForegroundColor = color;
            }

            defaultPath = "./SKONTest.skon";

            Console.Write("SKON input file?:");

            filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            sw.Reset();

            sw.Start();
            SKONObject skon = SKON.LoadFile(filePath);
            sw.Stop();

            Console.WriteLine("Successfully parsed SKON file in {0}ms!", sw.ElapsedMilliseconds);

            Console.WriteLine();

            Console.Write("Show SKON? Y/N (N):");

            if (Console.ReadLine() == "Y")
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine(SKON.Write(skon));

                Console.ForegroundColor = color;
            }

            Console.WriteLine();

            Console.Write("Run SKEMATests? Y/N (Y):");

            string ans = Console.ReadLine();

            if (ans.Length == 0 || ans == "Y")
            {
                sw.Reset();

                sw.Start();

                bool result = skema.Valid(skon);

                sw.Stop();

                if (result == true)
                {
                    Console.WriteLine("Successfully verified SKON file in {0}ms!", sw.ElapsedMilliseconds);
                }
                else
                {
                    Console.WriteLine("Failed to verify SKON file. {0}ms!", sw.ElapsedMilliseconds);
                }

                Console.WriteLine();
            }

            Console.ReadKey(true);
        }
    }
}
