namespace CodeExercise.Model;

/// <summary>
/// Represents a location
/// </summary>
public interface ILocation
{
    string Address { get; }
    double Latitude { get; }
    double Longitude { get; }
}

public class Location : ILocation
{
    public string Address { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}