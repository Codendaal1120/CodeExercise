using System.Diagnostics;
using System.Linq;
using CodeExercise.LocationService;
using CodeExercise.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace CodeExercise.LocationRepository.Test
{
    public class BasicSearchAccuracyTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test search accuracy
        /// </summary>
        [Test(Description = "Test the basic search accuracy within 500 meters")]
        public void TestBasicSearchAccuracyWithin100M()
        {
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader());

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

            repo.AddLocation(location450MAway);
            repo.AddLocation(location550MAway);

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
        [Test(Description = "Test the basic search accuracy within 1500 meters")]
        public void TestBasicSearchAccuracyWithin1500M()
        {
            var repo = new LocationRepo(new NullLogger<LocationRepo>(), new TestCsvLocationDataLoader());

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

            repo.AddLocation(location450MAway);
            repo.AddLocation(location550MAway);
            repo.AddLocation(location1000MAway);
            repo.AddLocation(location1600MAway);

            var ls = CreateLocationService(repo);
            var results = ls.GetLocations(refLocation, 1500, 10);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value!.ToArray();

            // Since we know that the locations added are way out of the original list (location wise),
            // the control test should only return the 3 test location within range
            Assert.AreEqual(3, resultArray.Length, "Expected test values to be returned");

            Assert.AreEqual(location450MAway.Address, resultArray[0].Address, "Only valid result was expected to be the control location within 500m");
        }
    }
    
}