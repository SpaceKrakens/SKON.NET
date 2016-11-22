#region LICENSE
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="SpaceKrakens">
//   MIT License
//   Copyright (c) 2016 SpaceKrakens
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace VerboseScannerParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SKON;
    using SKON.Internal;

    class Program
    {
        static void Main(string[] args)
        {
            string defaultPath = "./SKONTest.skon";

            Console.Write("Input file?:");

            string filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            SKONObject obj = VerboseParseFile(filePath);

            Console.WriteLine();

            Console.WriteLine(SKON.Write(obj));

            Console.ReadKey(true);
        }

        public static SKONObject VerboseParseFile(string path)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine("Could not find file {0}!", path);
                return new SKONObject();
            }

            Console.WriteLine("Found file {0}", path);

            Console.WriteLine("============================================");

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine(File.ReadAllText(path));

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("============================================");

            Console.WriteLine();

            Console.WriteLine("Creating scanner from file...");
            Scanner sc = new Scanner(path);
            
            Console.WriteLine("Created scanner with Buffer type of: {0}!", sc.buffer.GetType());

            Console.WriteLine();
            Console.WriteLine();

            int tokens = 0;

            Token t = new Token();
            while ((t = sc.Scan()).kind != 0)
            {
                Console.WriteLine("Reading a token of type {0} with value \"{1}\".", t.kind, t.val);
                Console.WriteLine();
                tokens++;
            }

            Console.WriteLine();

            Console.WriteLine("Done tokenizing file! Got {0} tokens!", tokens);

            sc = new Scanner(path);

            Parser parser = new Parser(sc);

            parser.Parse();
            
            return parser.data;
        }
    }
}
