using CodeExercise.Model;
using Microsoft.Extensions.Logging;

namespace CodeExercise.LocationRepository
{
    /// <inheritdoc cref="ILocationRepository"/>
    public class LocationRepo : ILocationRepository
    {
        private readonly ILogger<LocationRepo> _logger;
        private readonly List<ILocation> _locations;

        /// <summary>
        /// Create instance of the LocationRepo
        /// </summary>
        /// <param name="logger">Default logger</param>
        /// <param name="dataProvider">Data source provider</param>
        public LocationRepo(ILogger<LocationRepo> logger, ILocationDataProvider dataProvider)
        {
            if (dataProvider == null) throw new ArgumentNullException(nameof(dataProvider));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _locations = dataProvider.GetData().ToList();
        }

        //https://medium.com/@alexander.mueller/experiments-with-in-memory-spatial-radius-queries-in-python-e40c9e66cf63
        //public IEnumerable<ILocation> GetLocations(ILocation location, int maxDistance, int maxResults)
        //{
        //    return GetLocationsBasicSearch(location, maxDistance, maxResults);
        //}

        /// <inheritdoc/>
        public IEnumerable<ILocation> GetLocationsBasicSearch(ILocation location, int maxDistance, int maxResults)
        {
            var results = _locations.Select(x =>
                {
                    var dist = CalculateDistanceInMeters(location, x);

                    // Create a new object with the distance stored as
                    // we do not want to calculate this twice
                    return dist <= maxDistance
                        ? new SearchLocation(x, dist)
                        : null;
                })
                .Where(x => x != null)                                                         
                .OrderBy(x => x!.DistanceToSearchLocation)
                .Take(maxResults);

            return results!;
        }

        public void AddLocation(ILocation location)
        {
            _locations.Add(location);
        }

        public IEnumerable<ILocation> GetLocationsGeoHash(ILocation location, int maxDistance, int maxResults)
        {
            return Array.Empty<ILocation>();
        }

        // Since this calculation is only done here, we can isolate it,
        // if its needed elsewhere it can be changed into an extension or static utility
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

        private void BuildGeoIndex()
        {
            //https://github.com/Postlagerkarte/geohash-dotnet
        }
    }
}