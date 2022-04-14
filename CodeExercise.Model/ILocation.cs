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

/// <summary>
/// Runtime POCO for location
/// </summary>
public class Location : ILocation
{
    public string Address { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}