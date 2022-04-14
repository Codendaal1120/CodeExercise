using System.Diagnostics.CodeAnalysis;

namespace CodeExercise.Model
{
    /// <summary>
    /// Service used to search for locations
    /// </summary>
    public interface ILocationSearchService
    {
        /// <summary>
        /// Search locations
        /// </summary>
        /// <param name="location">The reference location</param>
        /// <param name="maxDistance">Max distance in meters</param>
        /// <param name="maxResults">Maximum number of results</param>
        SearchResults<IEnumerable<ISearchLocation>> GetLocations(ILocation location, int maxDistance, int maxResults);
    }
}
