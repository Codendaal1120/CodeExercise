using System.ComponentModel.DataAnnotations;
using CodeExercise.Api.Model;
using CodeExercise.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeExercise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly ILocationSearchService _locationSearchService;

        public LocationsController(ILogger<LocationsController> logger, ILocationSearchService locationSearchService)
        {
            _logger = logger;
            _locationSearchService = locationSearchService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(SearchResultsApi), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchLocations([Required] double longitude, [Required] double latitude, int maxDistance = 50, int maxResults = 25)
        {
            var results = _locationSearchService.GetLocations(
                new Location() {Latitude = latitude, Longitude = longitude},
                maxDistance,
                maxResults);

            return results.Success
                ? Ok(results.Value.Select(x => new SearchResultsApi(x)))
                : BadRequest(results.ErrorMessage);
        }
    }
}