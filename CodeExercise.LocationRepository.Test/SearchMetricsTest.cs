using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using CodeExercise.Model;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace CodeExercise.LocationRepository.Test
{
    [Ignore("Run manually to gather metrics")]
    public class SearchMetricsTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test(Description = "Get search performance results, not part of automated tests")]
        public void MetricsTest()
        {
            var basicStats = TestSearch(new LocationRepoSettings());
            var advancedStats = TestSearch(new LocationRepoSettings() { UseGeoHashing = true });
            var headerLine = "";
            var basicLine = "BasicTicks";
            var advanceLine = "AdvancedTicks";
            var print = string.Empty;

            foreach (var key in basicStats.Keys)
            {
                headerLine += $",{key * 168892}";
                basicLine += $",{basicStats[key]}";
                advanceLine += $",{advancedStats[key]}";
            }

            print += $"{headerLine}\n";
            print += $"{basicLine}\n";
            print += $"{advanceLine}\n";

            Assert.Pass(print);
        }

        private Dictionary<int, long> TestSearch(LocationRepoSettings settings)
        {
            var maxResults = 10;
            var stats = new Dictionary<int, long>();
            var data = new TestCsvLocationDataLoader().GetData().ToArray();
            var rnd = new Random();

            for (var i = 1; i < 100; i += 5)
            {
                var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader(multiplyLocations:i), settings);
                var ls = CreateLocationService(repo);

                var timer = new Stopwatch();
                
                timer.Start();

                var resultArray = PerformSearch(ls, maxResults, data[rnd.Next(0, data.Length)]);

                timer.Stop();

                if (resultArray.Count < 1)
                {
                    stats.Add(i, -1);
                    continue;
                }

                stats.Add(i, timer.ElapsedTicks);
            }

            return stats;
        }


        private IReadOnlyCollection<ILocation> PerformSearch(ILocationSearchService ls, int maxResults, ILocation? searchLocation = null)
        {
            searchLocation ??= new Location()
            {
                Address = "TEST",
                Latitude = 51.9055491,
                Longitude = 6.1174997
            };

            var results = ls.GetLocations(searchLocation, 1000, maxResults);

            Assert.True(results.Success, results.ErrorMessage);

            return results.Value!.ToArray();
        }

    
    }
}