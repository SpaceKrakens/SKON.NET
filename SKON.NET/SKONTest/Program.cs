using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKON.NET;
using System.IO;
using System.Globalization;

namespace SKONTest
{
    class Program
    {
        static string indentToken = "\t:";
        
        static void Main(string[] args)
        {
            string fileName = "C:\\Test\\SKONTest.skon";

            Console.WriteLine("Found " + fileName + ": " + File.Exists(fileName));

            Map map = Skon.LoadFile(fileName);

            Console.WriteLine("Successfully parsed file!");

            Console.WriteLine();
            Console.WriteLine();
            
            PrintObject(map, 0);
            
            Console.ReadKey(true);
        }

        static void PrintMap(Map map, int indent)
        {
            string indentString = "";
            for (int i = 0; i < indent; i++)
            {
                indentString += indentToken;
            }

            Console.WriteLine(indentString + " Map[" + map.Keys().Count + "] {");

            foreach (var key in map.Keys())
            {
                Console.WriteLine(indentString + " " + key + ":");

                PrintObject(map[key], indent + 1);
            }

            Console.WriteLine(indentString + " },\n");
        }

        static void PrintArray(SKON.NET.Array array, int indent)
        {
            string indentString = "";
            for (int i = 0; i < indent; i++)
            {
                indentString += indentToken;
            }

            Console.WriteLine(indentString + " Array[" + array.Length + "] [");

            for (int i = 0; i < array.Length; i++)
            {
                PrintObject(array[i], indent + 1);
            }

            Console.WriteLine(indentString + " ],\n");
        }

        static void PrintObject(SKONObject obj, int indent)
        {
            string indentString = "";
            for (int i = 0; i < indent; i++)
            {
                indentString += indentToken;
            }
            if (obj is Map)
            {
                PrintMap(obj as Map, indent);
            }
            else if (obj is SKON.NET.Array)
            {
                PrintArray(obj as SKON.NET.Array, indent);
            }
            else
            {
                Console.WriteLine(indentString + " " + obj + "\n");
            }
        }
    }
}
