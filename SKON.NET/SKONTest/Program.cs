using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SKON;

namespace SKONTest
{
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

            Stopwatch sw = new Stopwatch();

            sw.Start();

            SKONObject data = SKON.SKON.LoadFile(filePath);

            sw.Stop();

            Console.WriteLine("Successfully parsed file in {0}ms", sw.ElapsedMilliseconds);

            Console.WriteLine(SKON.SKON.Write(data));

            Console.ReadKey();
        }
    }
}
