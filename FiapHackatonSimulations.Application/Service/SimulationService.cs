using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace FiapHackatonSimulations.Application.Service;

public class SimulationService(ISimulationRepository simulationRepository) : ISimulationService
{
    public Task<IActionResult> GetPlotsById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> GetPlotsPaginated(int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}
