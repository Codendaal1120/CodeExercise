namespace CodeExercise.Model;

/// <summary>
/// Runtime POCO for location
/// </summary>
public class Location : ILocation
{
    public string Address { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}