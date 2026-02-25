using FiapHackatonSimulations.Domain.DTO;
using FiapHackatonSimulations.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace FiapHackatonSimulations.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class SimulationController(ISimulationService simulationService,
                                  IRabbitMQService rabbitMQService) : ControllerBase
{
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPlotsPaginated([FromQuery] short page = 0, [FromQuery] short pageSize = 10) =>
        await simulationService.GetPlotsPaginated(page, pageSize);

    [HttpGet("{plotId:guid}")]
    public async Task<IActionResult> GetPlotsByID(Guid plotId) =>
        await simulationService.GetPlotsById(plotId);

    [HttpPost]
    public async Task<IActionResult> PostSimulationsData([FromBody] SimulationDto req)
    {
        await rabbitMQService.PublishAsync("simulation-queue", req);

        return new AcceptedResult();
    }
}
