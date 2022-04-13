namespace CodeExercise.Model
{
    /// <summary>
    /// Storage interface for locations
    /// </summary>
    public interface ILocationRepository
    {
        IEnumerable<ILocation> GetLocationsBasicSearch(ILocation location, int maxDistance, int maxResults);
    }

}