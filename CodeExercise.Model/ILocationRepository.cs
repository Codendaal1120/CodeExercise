namespace CodeExercise.Model
{
    /// <summary>
    /// Storage interface for locations
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Search locations
        /// </summary>
        /// <param name="location">Reference location</param>
        /// <param name="maxDistance">Max distance from location</param>
        /// <param name="maxResults">Maximum number of results</param>
        /// <returns></returns>
        IEnumerable<ISearchLocation> GetLocations(ILocation location, int maxDistance, int maxResults);

        /// <summary>
        /// Add a new location to the repository
        /// </summary>
        void AddLocations(IReadOnlyCollection<ILocation> locations);
    }
}