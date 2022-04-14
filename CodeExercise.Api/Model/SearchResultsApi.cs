using CodeExercise.Model;
using Newtonsoft.Json;

namespace CodeExercise.Api.Model
{
    // Typically this model will be decorated with JSON annotations for serialization

    /// <summary>
    /// Represents a location
    /// </summary>
    [JsonObject("Location")]
    public class LocationApi : ILocation
    {
        /// <summary>
        /// Location text address
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Location Latitude
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Location Longitude
        /// </summary>
        public double Longitude { get; }
        
        /// <summary>
        /// Distance to the reference location
        /// </summary>
        public double Distance { get; }

        /// <summary/>
        /// <exception cref="ArgumentNullException"></exception>
        public LocationApi(ISearchLocation loc)
        {
            if (loc == null) throw new ArgumentNullException(nameof(loc));

            Address = loc.Address;
            Latitude = loc.Latitude;
            Longitude = loc.Longitude;
            Distance = loc.Distance;
        }
    }
}
