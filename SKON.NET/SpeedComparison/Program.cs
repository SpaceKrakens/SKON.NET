
namespace SpeedComparison
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using SKON;

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
                Version = string.Empty,
                VersionName = string.Empty,
                Author = new { },
                ExampleString = string.Empty,
                ExampleInteger = 0,
                ExampleDouble = 0.0,
                ExampleBoolean = false,
                ExampleDatetime = string.Empty,
                ExampleArray = new string[] { },
                ExampleMap = new {
                    ThisIsAKey = string.Empty,
                    ThisIsAnotherKey = 0
                },
                ArrayOfMaps = new dynamic[]{
                    new { Key = string.Empty },
                    new { Key = string.Empty },
                    new { AnotherKey = string.Empty }
                },
                MapOfAllDataTypes = new {
                    String = string.Empty,
                    Integer = 0,
                    Double = 0.0,
                    Boolean = false,
                    DateTime = string.Empty,
                    Array = new dynamic[] { },
                    Map = new { }
                },
                ArrayOfArrayOfStrings = new[] {
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty },
                    new[] { string.Empty, string.Empty }
                }
            };

            double jsonNMean = Clock.BenchmarkCpu(() => { JsonConvert.DeserializeAnonymousType(json, definition); });
            
            Console.WriteLine("Normalized mean: {0}ms", jsonNMean);
            
            Console.WriteLine();
            
            Console.WriteLine("SKON: {0:F3}, JSON: {1:F3}", skonNMean, jsonNMean);

            Console.WriteLine();

            double ratio = (skonNMean > jsonNMean) ? skonNMean / jsonNMean : jsonNMean / skonNMean;
            
            Console.WriteLine("Skon is " + ratio.ToString("F3") + "x " + ((skonNMean < jsonNMean) ? "faster" : (skonNMean == jsonNMean) ? "equal" : "slower") + " than json at this time!\n\n");

            SKONObject skonObject = TestSKON.TestSKONObject;

            Console.WriteLine(SKON.Write(skonObject));
            
            Console.ReadKey(true);
        }
    }
}
