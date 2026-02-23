using FiapHackatonSimulations.Domain.DTO;
using FiapHackatonSimulations.Domain.Interface.RabbitMQ;
using FiapHackatonSimulations.Domain.Interface.Service;

namespace FiapHackatonSimulations.Application.Service.RabbitMQ;

public class RabbitMQReceiver(ISimulationService simulationService) : IRabbitMQReceiver
{
    public async Task HandleAsync(SimulationDto message) =>
        await simulationService.PostSimulationsData(message);
}
