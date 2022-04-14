using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CodeExercise.Model;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace CodeExercise.LocationRepository.Test
{
    public class AdvancedSearchAccuracyTest : TestBase
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Test advanced search accuracy, building the geo index will allow us to use the geoIndex to search
        /// </summary>
        [Test(Description = "Test the advanced search accuracy within 1 meter")]
        public void TestAdvancedSearchAccuracyWithin1M()
        {
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader(), new LocationRepoSettings() { UseGeoHashing = true });

            var refLocation = new Location()
            {
                Address = "REF",
                Latitude = -33.92947021141639,
                Longitude = 18.5263035154
            };

            var location1MAway = new Location()
            {
                Address = "1Test",
                Latitude = -33.92947188053884,
                Longitude = 18.526305527056785
            };

            var location5MAway = new Location()
            {
                Address = "5Test",
                Latitude = -33.92945685843556,
                Longitude = 18.526356489028682
            };

            repo.AddLocations(new []{ location1MAway, location5MAway });

            var ls = CreateLocationService(repo);

            var results = ls.GetLocations(refLocation, 1, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 1 test location
            Assert.AreEqual(1, resultArray.Length, "Expected test values to be returned");

            Assert.AreEqual(location1MAway.Address, resultArray[0].Address, "Only valid result was expected to be the control location within 1m");

        }

        /// <summary>
        /// Test advanced search accuracy
        /// </summary>
        [Test(Description = "Test the advanced search accuracy within 500 meters")]
        public void TestAdvancedSearchAccuracyWithin500M()
        {
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader(), new LocationRepoSettings() { UseGeoHashing = true });

            var refLocation = new Location()
            {
                Address = "REF",
                Latitude = -33.92950987391598,
                Longitude = 18.526343615045644
            };

            var location450MAway = new Location()
            {
                Address = "450Test",
                Latitude = -33.92874765684919,
                Longitude = 18.530994200682663
            };

            var location550MAway = new Location()
            {
                Address = "550Test",
                Latitude = -33.92859944718297,
                Longitude = 18.532046802370058
            };

            repo.AddLocations(new[] { location450MAway, location550MAway });

            var ls = CreateLocationService(repo);

            var results = ls.GetLocations(refLocation, 500, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 1 test location
            Assert.AreEqual(1, resultArray.Length, "Expected test values to be returned");

            Assert.AreEqual(location450MAway.Address, resultArray[0].Address, "Only valid result was expected to be the control location within 500m");
        }

        /// <summary>
        /// Test search accuracy
        /// </summary>
        [Test(Description = "Test the advanced search accuracy within 1500 meters")]
        public void TestAdvancedSearchAccuracyWithin1500M()
        {
            //var repo = GetRepo();
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestEmptyLocationDataLoader(), new LocationRepoSettings() { UseGeoHashing = true, HashingPrecision = 2 });

            //-33.92950987391598, 18.526343615045644
            var refLocation = new Location()
            {
                Address = "REF",
                Latitude = -33.92950987391598,
                Longitude = 18.526343615045644
            };

            var location450MAway = new Location()
            {
                Address = "450Test",
                Latitude = -33.92874765684919,
                Longitude = 18.530994200682663
            };

            var location550MAway = new Location()
            {
                Address = "550Test",
                Latitude = -33.92859944718297,
                Longitude = 18.532046802370058
            };

            //-33.9276330873692, 18.536896744143547
            var location1000MAway = new Location()
            {
                Address = "1000Test",
                Latitude = -33.9276330873692,
                Longitude = 18.536896744143547
            };

            var location1600MAway = new Location()
            {
                Address = "1600Test",
                Latitude = -33.92678737618233,
                Longitude = 18.543655910826285
            };

            repo.AddLocations(new[] { location450MAway, location550MAway, location1000MAway, location1600MAway });

            var ls = CreateLocationService(repo);
            var results = ls.GetLocations(refLocation, 1500, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 3 test location within range
            Assert.AreEqual(3, resultArray.Length, "Expected test values to be returned");

            Assert.False(resultArray.Any(x => x.Address == location1600MAway.Address), "Results should not contain locations exceeding the max distance.");
        }

        /// <summary>
        /// Test search accuracy
        /// </summary>
        [Test(Description = "Test the advanced search accuracy within 50 km")]
        public void TestAdvancedSearchAccuracyWithin50Km()
        {
            //var repo = GetRepo();
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader(), new LocationRepoSettings() { UseGeoHashing = true });

            //-33.92950987391598, 18.526343615045644
            var refLocation = new Location()
            {
                Address = "REF",
                Latitude = -33.92950987391598,
                Longitude = 18.526343615045644
            };

            //-33.95635769828801, 18.722538116524348
            var location20KmAway = new Location()
            {
                Address = "20kmTest",
                Latitude = -33.95635769828801,
                Longitude = 18.722538116524348
            };

            //-33.93985165204204, 18.840194327759427
            var location30KmAway = new Location()
            {
                Address = "30kmTest",
                Latitude = -33.93985165204204,
                Longitude = 18.840194327759427
            };

            //-33.74051211600767, 19.018672326193002
            var location55KmAway = new Location()
            {
                Address = "55kmTest",
                Latitude = -33.74051211600767,
                Longitude = 19.018672326193002
            };

            repo.AddLocations(new[] { location20KmAway, location30KmAway, location55KmAway });

            var ls = CreateLocationService(repo);
            var results = ls.GetLocations(refLocation, 50000, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 3 test location within range
            Assert.AreEqual(2, resultArray.Length, "Expected test values to be returned");

            Assert.False(resultArray.Any(x => x.Address == location55KmAway.Address), "Results should not contain locations exceeding the max distance.");
        }

        /// <summary>
        /// Test search accuracy
        /// </summary>
        [Test(Description = "Test the advanced search accuracy within 200 km")]
        public void TestAdvancedSearchAccuracyWithin500Km()
        {
            //var repo = GetRepo();
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader(), new LocationRepoSettings() { UseGeoHashing = true });

            //-33.92950987391598, 18.526343615045644
            var refLocation = new Location()
            {
                Address = "REF",
                Latitude = -33.92950987391598,
                Longitude = 18.526343615045644
            };

            //-33.5834738572646, 19.2812195765591
            var location80KmAway = new Location()
            {
                Address = "80kmTest",
                Latitude = -33.5834738572646,
                Longitude = 19.2812195765591
            };

            //-33.897113506360284, 20.17974865060645
            var location200KmAway = new Location()
            {
                Address = "200kmTest",
                Latitude = -33.897113506360284,
                Longitude = 20.17974865060645
            };

            //-34.10075946208508, 20.777346990632868
            var location250KmAway = new Location()
            {
                Address = "250kmTest",
                Latitude = -34.10075946208508,
                Longitude = 20.777346990632868
            };
            
            repo.AddLocations(new[] { location80KmAway, location200KmAway, location250KmAway });

            var ls = CreateLocationService(repo);
            var results = ls.GetLocations(refLocation, 200 * 1000, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 3 test location within range
            Assert.AreEqual(2, resultArray.Length, "Expected test values to be returned");

            Assert.False(resultArray.Any(x => x.Address == location250KmAway.Address), "Results should not contain locations exceeding the max distance.");
        }
    }
    
}