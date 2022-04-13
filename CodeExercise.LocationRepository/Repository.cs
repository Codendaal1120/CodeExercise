using System.Globalization;
using CodeExercise.Model;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace CodeExercise.LocationRepository
{
    public class Repository : ILocationRepository
    {
        private readonly ILogger<Repository> _logger;
        private readonly IReadOnlyCollection<ILocation> _locations;
        
        /// <summary>
        /// Create instance of the Repository
        /// </summary>
        /// <param name="logger">Default logger</param>
        /// <param name="sourceFile">Path to the source csv</param>
        /// <param name="multiplyLocations">Testing parameter to duplicate the records</param>
        public Repository(ILogger<Repository> logger, string sourceFile = "locations.csv", int multiplyLocations = 1)
        {
            if (sourceFile == null) throw new ArgumentNullException(nameof(sourceFile));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            try
            {
                using var sr = new StreamReader(sourceFile);
                using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);
                _locations = csv.GetRecords<Location>().ToArray();

                if (multiplyLocations > 1)
                {
                    var list = _locations.ToList();
                    for (var i = 1; i < multiplyLocations; i++)
                    {
                        list.AddRange(_locations);
                    }

                    _locations = list;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to load locations : {Error}", e.Message);
                // rethrow as this is a critical failure
                throw;
            }
        }

        //https://medium.com/@alexander.mueller/experiments-with-in-memory-spatial-radius-queries-in-python-e40c9e66cf63
        //public IEnumerable<ILocation> GetLocations(ILocation location, int maxDistance, int maxResults)
        //{
        //    return GetLocationsBasicSearch(location, maxDistance, maxResults);
        //}

        public IEnumerable<ILocation> GetLocationsBasicSearch(ILocation location, int maxDistance, int maxResults)
        {
            var results = _locations.Select(x =>
                {
                    var dist = CalculateDistanceInMeters(location, x);

                    // Create a new object with the distance stored as
                    // we do not want to calculate this twice
                    return dist >= maxDistance
                        ? new SearchLocation(x, dist)
                        : null;
                })
                .Where(x => x != null)                                                         
                .OrderBy(x => x!.DistanceToSearchLocation)
                .Take(maxResults);

            return results!;
        }

        private double CalculateDistanceInMeters(ILocation loc1, ILocation loc2)
        {
            var rlat1 = Math.PI * loc1.Latitude / 180;
            var rlat2 = Math.PI * loc2.Latitude / 180;
            var rlon1 = Math.PI * loc1.Longitude / 180;
            var rlon2 = Math.PI * loc2.Longitude / 180;
            var theta = loc1.Longitude - loc2.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }
    }
}