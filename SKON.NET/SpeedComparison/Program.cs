
namespace SpeedComparison
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using SKON;
    using Newtonsoft.Json;
    
    class Program
    {
        static void Main(string[] args)
        {
            string skonPath = "./TestSKON.skon";

            string jsonPath = "./TestJSON.json";

            string skon = File.ReadAllText(skonPath);

            string json = File.ReadAllText(jsonPath);

            int tests = 100;

            long[] skonTimes = new long[tests];

            long[] jsonTimes = new long[tests];
            
            Console.WriteLine("=== SKON Tests ===");

            double skonNMean = Clock.BenchmarkCpu(() => SKON.Parse(skon));

            Console.WriteLine("Normalized mean: {0}ms", skonNMean);

            Console.WriteLine();

            Console.WriteLine("=== JSON Tests ====");

            var definition = new {
                Version = "",
                VersionName = "",
                Author = new { },
                ExampleString = "",
                ExampleInteger = 0,
                ExampleDouble = 0.0,
                ExampleBoolean = false,
                ExampleDatetime = "",
                ExampleArray = new string[] { },
                ExampleMap = new {
                    ThisIsAKey = "",
                    ThisIsAnotherKey = 0,
                },
                ArrayOfMaps = new dynamic[]{
                    new { Key = "" },
                    new { Key = "" },
                    new { AnotherKey = "" },
                },
                MapOfAllDataTypes = new {
                    String = "",
                    Integer = 0,
                    Double = 0.0,
                    Boolean = false,
                    DateTime = "",
                    Array = new dynamic[] { },
                    Map = new { },
                },
                ArrayOfArrayOfStrings = new string[][] {
                    new string[] { "", "" },
                    new string[] { "", "" },
                    new string[] { "", "" },
                }
            };

            double jsonNMean = Clock.BenchmarkCpu(() => { JsonConvert.DeserializeAnonymousType(json, definition); });
            
            Console.WriteLine("Normalized mean: {0}ms", jsonNMean);
            
            Console.WriteLine();
            
            Console.WriteLine("SKON: {0:F3}, JSON: {1:F3}", skonNMean, jsonNMean);

            Console.WriteLine();

            double ratio = (skonNMean > jsonNMean) ? skonNMean / jsonNMean : jsonNMean / skonNMean;
            
            Console.WriteLine("Skon is " + (ratio.ToString("F3")) + "x " + ((skonNMean < jsonNMean) ? "faster" : (skonNMean == jsonNMean) ? "equal" : "slower") + " than json at this time!");
            
            Console.ReadKey(true);
        }
    }
}
