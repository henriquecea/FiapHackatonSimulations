using FiapHackatonSimulations.Domain.DTO;
using FiapHackatonSimulations.Domain.Entity;
using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FiapHackatonSimulations.Application.Service;

public class SimulationService(ISimulationRepository simulationRepository,
                               ILogger<SimulationService> logger) : ISimulationService
{
    public async Task<IActionResult> GetPlotsById(Guid id)
    {
        try
        {
            var res = await simulationRepository.GetByPlotIdAsync(id);

            if (res is null)
                return new NotFoundResult();

            return new OkObjectResult(new
            {
                res
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar SensorData para Plot {Plot}", id);

            return new BadRequestObjectResult(ex.Message);
        }
    }

    public Task<IActionResult> GetPlotsPaginated(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> PostSimulationsData(SimulationDto req)
    {
        try
        {
            var entity = new SensorData
            {
                Plot = req.Plot,
                PrecipitationLevel = req.PrecipitationLevel,
                SoilMoisture = req.SoilMoisture,
                Temperature = req.Temperature
            };

            await simulationRepository.AddAsync(entity);
            await simulationRepository.SaveChangesAsync();

            logger.LogInformation(
                "SensorData {SensorDataId} criada com sucesso",
                entity.Id);

            return new AcceptedResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar SensorData para Plot {Plot}", req.Plot);

            return new BadRequestObjectResult(ex.Message);
        }
    }
}
