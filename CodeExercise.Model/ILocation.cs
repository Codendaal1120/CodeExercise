namespace CodeExercise.Model;

/// <summary>
/// Represents a location
/// </summary>
public interface ILocation
{
    /// <summary>
    /// Location text address
    /// </summary>
    string Address { get; }

    /// <summary>
    /// Location Latitude
    /// </summary>
    double Latitude { get; }

    /// <summary>
    /// Location Longitude
    /// </summary>
    double Longitude { get; }
}