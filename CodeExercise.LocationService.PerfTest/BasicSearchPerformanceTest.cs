using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CodeExercise.LocationRepository;
using CodeExercise.Model;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace CodeExercise.LocationService.PerfTest
{
    public class BasicSearchPerformanceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Base test check the default repo for max 10 results
        /// </summary>
        [Test(Description = "Search the default repo of 168 891 records for maximum of 10")]
        public void SearchDefaultRepoFor10Results()
        {
            var ls = new LocationSearchService(
                new NullLogger<LocationSearchService>(),
                new LocationRepository.LocationRepo(
                    new NullLogger<LocationRepository.LocationRepo>()));

            var timer = new Stopwatch();

            timer.Start();

            var maxResults = 10;
            var resultArray = PerformSearch(ls, maxResults);

            Assert.GreaterOrEqual(resultArray.Count, 1);
            Assert.LessOrEqual(resultArray.Count, maxResults);

            timer.Stop();

            Assert.Pass("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
        }

        /// <summary>
        /// Test the effect of increasing the max record count
        /// </summary>
        [Test(Description = "Search the default repo of 168 891 records for maximum of 50")]
        public void SearchDefaultRepoFor50Results()
        {
            var ls = new LocationSearchService(
                new NullLogger<LocationSearchService>(),
                new LocationRepository.LocationRepo(
                    new NullLogger<LocationRepository.LocationRepo>()));

            var timer = new Stopwatch();

            timer.Start();

            var maxResults = 50;
            var resultArray = PerformSearch(ls, maxResults);

            Assert.GreaterOrEqual(resultArray.Count, 1);
            Assert.LessOrEqual(resultArray.Count, maxResults);

            timer.Stop();

            Assert.Pass("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
        }

        /// <summary>
        /// Duplicate the repo 10 times, should slow down query
        /// </summary>
        [Test(Description = "Search the default repo of 168 891 * 10 records for maximum of 50")]
        public void Search10XRepoFor50Results()
        {
            var ls = new LocationSearchService(
                new NullLogger<LocationSearchService>(),
                new LocationRepository.LocationRepo(
                    new NullLogger<LocationRepository.LocationRepo>(), multiplyLocations:10));

            var timer = new Stopwatch();

            timer.Start();

            var maxResults = 50;
            var resultArray = PerformSearch(ls, maxResults);

            Assert.GreaterOrEqual(resultArray.Count, 1);
            Assert.LessOrEqual(resultArray.Count, maxResults);

            timer.Stop();

            Assert.Pass("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
        }

        /// <summary>
        /// Duplicate the repo 100 times, should slow down query
        /// </summary>
        [Test(Description = "Search the default repo of 168 891 * 100 records for maximum of 50")]
        public void Search100XRepoFor50Results()
        {
            var ls = new LocationSearchService(
                new NullLogger<LocationSearchService>(),
                new LocationRepository.LocationRepo(
                    new NullLogger<LocationRepository.LocationRepo>(), multiplyLocations: 100));

            var timer = new Stopwatch();

            timer.Start();

            var maxResults = 50;
            var resultArray = PerformSearch(ls, maxResults);

            Assert.GreaterOrEqual(resultArray.Count, 1);
            Assert.LessOrEqual(resultArray.Count, maxResults);

            timer.Stop();

            Assert.Pass("Time taken: " + timer.Elapsed.ToString(@"m\:ss\.fff"));
        }

        private IReadOnlyCollection<ILocation> PerformSearch(ILocationSearchService ls, int maxResults)
        {
            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 51.9055491,
                Longitude = 6.1174997
            };

            var results = ls.GetLocations(sample, 10, maxResults);

            Assert.True(results.Success, results.ErrorMessage);

            return results.Value.ToArray();
        }
    }
}