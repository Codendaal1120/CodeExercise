namespace CodeExercise.LocationRepository;

public class LocationRepoSettings
{
    /// <summary>
    /// Indicate weather to use the geo hashing search
    /// </summary>
    public bool UseGeoHashing { get; set; } = false;

    /// <summary>
    /// Higher precision means smaller geo fence. If the greater the max distances searched are, the lower this should be
    /// </summary>
    public int HashingPrecision { get; set; } = 6;
}