using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CodeExercise.Model;
using CsvHelper;

namespace CodeExercise.LocationRepository.Test;

internal class TestCsvLocationDataLoader : ILocationDataProvider
{
    private readonly string _sourceFile;
    private readonly int _multiplyLocations;

    /// <summary>
    /// Create instance of the TestCsvLocationDataLoader
    /// </summary>
    /// <param name="sourceFile">Path to the source csv</param>
    /// <param name="multiplyLocations">Testing parameter to duplicate the records</param>
    public TestCsvLocationDataLoader(string sourceFile = "locations.csv", int multiplyLocations = 1)
    {
        _sourceFile = sourceFile;
        _multiplyLocations = multiplyLocations;
    }

    public IEnumerable<ILocation> GetData()
    {
        try
        {
            using var sr = new StreamReader(_sourceFile);
            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);
            var locations = csv.GetRecords<Location>().ToArray();

            if (_multiplyLocations > 1)
            {
                var list = new List<Location>();
                for (var i = 1; i < _multiplyLocations; i++)
                {
                    list.AddRange(locations);
                }

                return list;
            }

            return locations;
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to load locations : {0}", e.Message);
            // rethrow as this is a critical failure
            throw;
        }
    }
}