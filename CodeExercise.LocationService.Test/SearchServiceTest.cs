using System;
using System.Linq;
using CodeExercise.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace CodeExercise.LocationService.Test
{
    public class SearchServiceTest
    {
        private ILocationSearchService _locationSearchService;

        [SetUp]
        public void Setup()
        {
            var mockRepo = new Mock<ILocationRepository>();

            // For unit test just return a collection containing the supplied location
            mockRepo.Setup(x => x.GetLocationsBasicSearch(
                It.IsAny<ILocation>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .Returns((ILocation loc, int md, int mr) => new[] { loc });

            _locationSearchService = new LocationSearchService(new NullLogger<LocationSearchService>(), mockRepo.Object);
        }

        /// <summary>
        /// Test the basic search
        /// </summary>
        [Test(Description = "Test a basic search")]
        public void TestBasicSearch()
        {
            var maxResults = 10;

            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 51.9055491,
                Longitude = 6.1174997
            };

            var results = _locationSearchService.GetLocations(sample, 10, maxResults);

            Assert.True(results.Success, results.ErrorMessage);

            var resultArray = results.Value.ToArray();

            Assert.GreaterOrEqual(resultArray.Length, 1);
            Assert.LessOrEqual(resultArray.Length, maxResults);
        }

        /// <summary>
        /// A null input location should return an error
        /// </summary>
        [Test(Description = "Test a basic search with a NULL location")]
        public void TestBasicSearchWithNullLocation()
        {
            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 51.9055491,
                Longitude = 6.1174997
            };

            var results = _locationSearchService.GetLocations(null, 10, 1);

            Assert.False(results.Success, "Null input expected to fail");
            Assert.AreEqual("Invalid location", results.ErrorMessage);
        }

        /// <summary>
        /// An invalid input location should return an error
        /// </summary>
        [Test(Description = "Test a basic search with an invalid location")]
        public void TestBasicSearchWithInvalidLocation()
        {
            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 89,
                Longitude = 181
            };

            var results = _locationSearchService.GetLocations(sample, 10, 1);

            Assert.False(results.Success, "Invalid input expected to fail");
            Assert.AreEqual("Invalid location", results.ErrorMessage);
        }

        /// <summary>
        /// An invalid max distance should return an error
        /// </summary>
        [Test(Description = "Test a basic search with an invalid maxDistance")]
        public void TestBasicSearchWithInvalidMaxDistance()
        {
            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 51,
                Longitude = 6
            };

            var results = _locationSearchService.GetLocations(sample, -2, 1);

            Assert.False(results.Success, "Invalid max distance expected to fail");
            Assert.AreEqual("Invalid max distance", results.ErrorMessage);
        }

        /// <summary>
        /// An invalid max results should return an error
        /// </summary>
        [Test(Description = "Test a basic search with an invalid maxResults")]
        public void TestBasicSearchWithInvalidMaxResults()
        {
            var sample = new Location()
            {
                Address = "TEST",
                Latitude = 51,
                Longitude = 6
            };

            var results = _locationSearchService.GetLocations(sample, 1, -1);

            Assert.False(results.Success, "Invalid max results expected to fail");
            Assert.AreEqual("Invalid max results", results.ErrorMessage);
        }
    }
}