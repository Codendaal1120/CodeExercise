using System.ComponentModel.DataAnnotations;
using CodeExercise.Api.Model;
using CodeExercise.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeExercise.Api.Controllers
{
    /// <summary>
    /// Locations api
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly ILocationSearchService _locationSearchService;

        /// <summary/>
        public LocationsController(ILogger<LocationsController> logger, ILocationSearchService locationSearchService)
        {
            _logger = logger;
            _locationSearchService = locationSearchService;
        }

        /// <summary>
        /// Search for locations located near a target coordinates
        /// </summary>
        /// <param name="longitude">Target longitude</param>
        /// <param name="latitude">Target latitude</param>
        /// <param name="maxDistance">Max distance in meters from target location</param>
        /// <param name="maxResults">Maximum number of results</param>
        /// <returns>Collection of locations within the max distance from the target location</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IReadOnlyCollection<LocationApi>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchLocations(
            [Required] [Range(typeof(double), "-180.0", "180.0")] double longitude, 
            [Required] [Range(typeof(double), "-90.0", "90.0")] double latitude, 
            int maxDistance = 50, int maxResults = 25)
        {
            var results = _locationSearchService.GetLocations(
                new Location() {Latitude = latitude, Longitude = longitude},
                maxDistance,
                maxResults);

            return results.Success
                ? Ok(results.Value!.Select(x => new LocationApi(x)))
                : BadRequest(results.ErrorMessage);
        }
    }
}