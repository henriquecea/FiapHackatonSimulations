using Microsoft.AspNetCore.Mvc;

namespace FiapHackatonSimulations.Domain.Interface.Service;

public interface ISimulationService
{
    /// <summary>
    /// Busca simulações de Plots paginados.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<IActionResult> GetPlotsPaginated(int page, int pageSize);

    /// <summary>
    /// Busca simulações de Plots por ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IActionResult> GetPlotsById(Guid id);
}
