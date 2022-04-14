namespace CodeExercise.Model;

/// <summary>
/// Data source provider for location data
/// </summary>
public interface ILocationDataProvider
{
    /// <summary>
    /// Get the location data
    /// </summary>
    IEnumerable<ILocation> GetData();
}