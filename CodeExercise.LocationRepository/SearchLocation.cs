using CodeExercise.Model;

namespace CodeExercise.LocationRepository;

internal class SearchLocation : Location
{
    public double DistanceToSearchLocation { get; }

    public SearchLocation(ILocation location, double distance)
    {
        if (location == null) throw new ArgumentNullException(nameof(location));

        Address = location.Address;
        Latitude = location.Latitude;
        Longitude = location.Longitude;
        DistanceToSearchLocation = distance;
    }
}