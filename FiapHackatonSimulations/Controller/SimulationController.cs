using FiapHackatonSimulations.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FiapHackatonSimulations.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class SimulationController(ISimulationService simulationService)
{
    public async Task<IActionResult> GetPlotsPaginated([FromQuery] short page = 0, [FromQuery] short pageSize = 10) =>
        await simulationService.GetPlotsPaginated();
}
