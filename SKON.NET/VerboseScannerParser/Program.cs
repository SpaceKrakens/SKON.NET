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
    using SKON.SKEMA;
    using SKON.SKEMA.Internal;

    class Program
    {
        static void Main(string[] args)
        {
            string defaultPath = "./SKONTest.skon";

            Console.Write("SKON Input file?:");

            string filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            SKONObject obj = VerboseParseFile(filePath);

            Console.WriteLine();

            Console.WriteLine(SKON.Write(obj));

            Console.ReadKey(true);


            defaultPath = "./SKEMATest.skema";

            Console.Write("SKON Input file?:");

            filePath = Console.ReadLine();

            if (File.Exists(filePath) == false)
            {
                filePath = defaultPath;
            }

            SKEMAObject skemaObj = VerboseParseFileSKEMA(filePath);

            Console.WriteLine();

            Console.WriteLine(SKEMA.Write(skemaObj));

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
            global::SKON.Internal.Scanner sc = new global::SKON.Internal.Scanner(path);
            
            Console.WriteLine("Created scanner with Buffer type of: {0}!", sc.buffer.GetType());

            Console.WriteLine();
            Console.WriteLine();

            int tokens = 0;

            global::SKON.Internal.Token t = new global::SKON.Internal.Token();
            while ((t = sc.Scan()).kind != 0)
            {
                Console.WriteLine("Reading a token of type {0} with value \"{1}\".", t.kind, t.val);
                Console.WriteLine();
                tokens++;
            }

            Console.WriteLine();

            Console.WriteLine("Done tokenizing file! Got {0} tokens!", tokens);

            sc = new global::SKON.Internal.Scanner(path);

            global::SKON.Internal.Parser parser = new global::SKON.Internal.Parser(sc);

            parser.Parse();
            
            return parser.data;
        }

        public static SKEMAObject VerboseParseFileSKEMA(string path)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine("Could not find file {0}!", path);
                return null;
            }

            Console.WriteLine("Found file {0}", path);

            Console.WriteLine("============================================");

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine(File.ReadAllText(path));

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("============================================");

            Console.WriteLine();

            Console.WriteLine("Creating scanner from file...");
            global::SKON.SKEMA.Internal.Scanner sc = new global::SKON.SKEMA.Internal.Scanner(path);

            Console.WriteLine("Created scanner with Buffer type of: {0}!", sc.buffer.GetType());

            Console.WriteLine();
            Console.WriteLine();

            int tokens = 0;

            global::SKON.SKEMA.Internal.Token t = new global::SKON.SKEMA.Internal.Token();
            while ((t = sc.Scan()).kind != 0)
            {
                Console.WriteLine("Reading a token of type {0} with value \"{1}\".", t.kind, t.val);
                Console.WriteLine();
                tokens++;
            }

            Console.WriteLine();

            Console.WriteLine("Done tokenizing file! Got {0} tokens!", tokens);

            sc = new global::SKON.SKEMA.Internal.Scanner(path);

            global::SKON.SKEMA.Internal.Parser parser = new global::SKON.SKEMA.Internal.Parser(sc);

            parser.Parse();

            return parser.data;
        }
    }
}
