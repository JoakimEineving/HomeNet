using HomeNet.Services.WeatherService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeNet.Services.WeatherService.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        //TODO: Implement ex handling
        [HttpGet("forecast/{longitude}/{latitude}")]
        public async Task<IActionResult> GetWeatherAsync(double longitude, double latitude)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(latitude, longitude);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather for latitude: {latitude}, longitude: {longitude}", latitude, longitude);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
