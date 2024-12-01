using HamedStack.CQRS;
using HamedStack.TheResult.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Tributech.Application.Create;
using Tributech.Application.Delete;
using Tributech.Application.Select;
using Tributech.Application.SelectAll;
using Tributech.Application.Update;

namespace Tributech.Presentation.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> _logger;
        private readonly ICommandQueryDispatcher _dispatcher;

        public SensorController(ILogger<SensorController> logger, ICommandQueryDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensor([FromBody] SensorRequest request)
        {
            var output = await _dispatcher.Send(new CreateSensorCommand()
            {
                Location = request.Location,
                LowerWarningLimit = request.LowerWarningLimit,
                UpperWarningLimit = request.UpperWarningLimit,
                Name = request.Name
            });
            return output.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensor(Guid id, [FromBody] SensorRequest request)
        {
            var output = await _dispatcher.Send(new UpdateSensorCommand()
            {
                Id = id,
                Location = request.Location,
                LowerWarningLimit = request.LowerWarningLimit,
                UpperWarningLimit = request.UpperWarningLimit,
                Name = request.Name
            });
            return output.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(Guid id)
        {
            var output = await _dispatcher.Send(new DeleteSensorCommand()
            {
                Id = id
            });

            return output.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorById(Guid id)
        {
            var output = await _dispatcher.Send(new GetSensorQuery()
            {
                Id = id
            });

            return output.ToActionResult();
        }


        [HttpGet()]
        public async Task<IActionResult> GetSensorsAsync()
        {
            var output = await _dispatcher.Send(new GetSensorsQuery());

            return output.ToActionResult();
        }
    }
}
