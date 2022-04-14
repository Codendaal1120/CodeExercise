using CodeExercise.Model;
using Microsoft.Extensions.Logging;

namespace CodeExercise.LocationService
{
    /// <inheritdoc cref="ILocationSearchService"/>
    public class LocationSearchService : ILocationSearchService
    {
        private readonly ILogger<LocationSearchService> _logger;
        private readonly ILocationRepository _repo;

        public LocationSearchService(ILogger<LocationSearchService> logger, ILocationRepository repo)
        {
            _logger = logger;
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <inheritdoc/>
        public SearchResults<IEnumerable<ISearchLocation>> GetLocations(ILocation location, int maxDistance, int maxResults)
        {
            if (location == null!) return SearchResults<IEnumerable<ISearchLocation>>.Fail("Invalid location");
            if (maxDistance <= 0) return SearchResults<IEnumerable<ISearchLocation>>.Fail("Invalid max distance");
            if (maxResults <= 0) return SearchResults<IEnumerable<ISearchLocation>>.Fail("Invalid max results");

            if (!IsLocationValid(location))
            {
                return SearchResults<IEnumerable<ISearchLocation>>.Fail("Invalid location");
            }

            try
            {
                var locations = _repo.GetLocations(location, maxDistance, maxResults);
                return SearchResults<IEnumerable<ISearchLocation>>.Succeed(locations);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during search request {Error}", e.Message);
                return SearchResults<IEnumerable<ISearchLocation>>.Fail(e.Message);
            }
        }
        
        public static bool IsLocationValid(ILocation loc)
        {
            if (loc.Latitude is < -90 or > 90)
            {
                return false;
            }

            if (loc.Longitude is < -180 or > 180)
            {
                return false;
            }

            return true;

        }
    }
}