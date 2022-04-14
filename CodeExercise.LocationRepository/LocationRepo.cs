using CodeExercise.Model;
using Geohash;
using Microsoft.Extensions.Logging;
using NGeoHash;

namespace CodeExercise.LocationRepository
{
    /// <inheritdoc cref="ILocationRepository"/>
    public class LocationRepo : ILocationRepository
    {
        private readonly ILogger<LocationRepo> _logger;
        private readonly List<ILocation> _locations;
        private Dictionary<string, ILocation[]> _hashedLocations2;
        private readonly LocationRepoSettings _settings;
        
        /// <summary>
        /// Create instance of the LocationRepo
        /// </summary>
        /// <param name="logger">Default logger</param>
        /// <param name="dataProvider">Data source provider</param>
        /// <param name="settings">Config options for the LocationRepo</param>
        public LocationRepo(ILogger<LocationRepo> logger, ILocationDataProvider dataProvider, LocationRepoSettings? settings = null)
        {
            if (dataProvider == null) throw new ArgumentNullException(nameof(dataProvider));

            _hashedLocations2 = new Dictionary<string, ILocation[]>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _locations = dataProvider.GetData().ToList();
            _settings = settings ?? new LocationRepoSettings();

            if (_settings.UseGeoHashing)
            {
                BuildGeoIndex();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<ILocation> GetLocations(ILocation location, int maxDistance, int maxResults)
        {
            return _hashedLocations2.Any()
                ? GetLocationsGeoHash(location, maxDistance, maxResults)
                : GetLocationsBruteForce(_locations, location, maxDistance, maxResults);
        }

        public void AddLocations(IReadOnlyCollection<ILocation> locations)
        {
            _locations.AddRange(locations);

            // If we are using the geo index, we need to rebuild it now
            if (_settings.UseGeoHashing)
            {
                BuildGeoIndex();
            }
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

        // Build the geo index, see https://github.com/Postlagerkarte/geohash-dotnet
        // We compute the hash then store it in a dictionary
        private void BuildGeoIndex()
        {
            var hasher = new Geohasher();
            
            _hashedLocations2 = _locations
                .GroupBy(x => hasher.Encode(x.Latitude, x.Longitude, _settings.HashingPrecision))
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        private IEnumerable<ILocation> GetLocationsBruteForce(IEnumerable<ILocation> locations, ILocation location, int maxDistance, int maxResults)
        {
            var results = locations.Select(x =>
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

        private IEnumerable<ILocation> GetLocationsGeoHash(ILocation location, int maxDistance, int maxResults)
        {
            var hasher = new Geohasher();
            var locationHash = hasher.Encode(location.Latitude, location.Longitude, _settings.HashingPrecision);

            // try to reduce the scope 
            if (_hashedLocations2.TryGetValue(locationHash, out var foundLocations))
            {
                return GetLocationsBruteForce(foundLocations, location, maxDistance, maxResults);
            }

            // fallback to brute force
            return GetLocationsBruteForce(_locations, location, maxDistance, maxResults);
        }
        
    }
}