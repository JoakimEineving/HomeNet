using HomeNet.Core.Dtos.Lighting;
using HomeNet.Services.LightingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeNet.Services.LightingService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HueController : ControllerBase
    {
        private readonly ILogger<HueController> _logger;
        private readonly IHueService _hueService;

        public HueController(ILogger<HueController> logger,IHueService hueService)
        {
            _logger = logger;
            _hueService = hueService;
        }

        [HttpGet("lights")]
        public async Task<IActionResult> GetLights()
        {
            try
            {
                var lights = await _hueService.GetLightsAsync();
                return Ok(lights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lights");
                return StatusCode(500, "Internal Server Error");
            }
           
        }

        [HttpPut("lights/{lightNumber}/state")]
        public async Task<IActionResult> SetLightState(string lightNumber, [FromBody] LightStateDto state)
        {
            try
            {
                var result = await _hueService.SetLightStateAsync(lightNumber, state.on);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting light state for lightNumber: {lightNumber}", lightNumber);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}