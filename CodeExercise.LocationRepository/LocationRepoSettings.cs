namespace CodeExercise.LocationRepository;

public class LocationRepoSettings
{
    public bool UseGeoHashing { get; set; } = false;
    public int HashingPrecision { get; set; } = 6;
}