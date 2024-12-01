using Microsoft.AspNetCore.Mvc;
using Tributech.Domain.Services;

namespace Tributech.Presentation.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly ISensorDataService _sensorDataService;

        public ProxyController(ISensorDataService sensorDataService)
        {
            _sensorDataService = sensorDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetSensorData(Guid sensorId, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
        {
            try
            {
                var data = await _sensorDataService.GetSensorDataAsync(sensorId.ToString(), from, to);
                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Error accessing the platform: {ex.Message}");
            }
        }
    }
}
