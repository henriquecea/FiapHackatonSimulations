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
            var random = new Random();
            var simulations = new List<SensorData>();
            var simulationId = Guid.NewGuid();

            for (int i = 0; i < 25; i++)
            {
                var entity = new SensorData
                {
                    Plot = simulationId,

                    PrecipitationLevel = Math.Round(
                   (decimal)random.NextDouble() * 100m, 2), // 0 - 100 mm

                    SoilMoisture = Math.Round(
                   (decimal)random.NextDouble() * 60m, 2), // 0 - 60 %

                    Temperature = Math.Round(
                   (decimal)random.NextDouble() * 40m, 2) // 0 - 40 °C
                };

                await simulationRepository.AddAsync(entity);
            }

            await simulationRepository.SaveChangesAsync();

            logger.LogInformation(
                "SensorData {SensorDataId} criada com sucesso", simulationId);

            return new AcceptedResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar SensorData para Plot {Plot}", req.Plot);

            return new BadRequestObjectResult(ex.Message);
        }
    }
}
