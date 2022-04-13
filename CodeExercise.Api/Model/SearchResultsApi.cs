using CodeExercise.Model;

namespace CodeExercise.Api.Model
{
    // Typically this model will be decorated with JSON annotations for serialization

    /// <summary>
    /// Location search results
    /// </summary>
    public class SearchResultsApi : ILocation
    {
        public string Address { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public SearchResultsApi(ILocation loc)
        {
            if (loc == null) throw new ArgumentNullException(nameof(loc));

            Address = loc.Address;
            Latitude = loc.Latitude;
            Longitude = loc.Longitude;
        }
    }
}
