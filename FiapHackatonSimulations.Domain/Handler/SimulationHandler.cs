using FiapHackatonSimulations.Domain.DTO;
using FiapHackatonSimulations.Domain.Interface.RabbitMQ;
using Newtonsoft.Json;

namespace FiapHackatonSimulations.Domain.Handler;

public class SimulationHandler(IRabbitMQReceiver receiver) : IRabbitMQMessageHandler
{
    public string QueueName => "simulation-queue";

    public async Task HandleAsync(string message)
    {
        var dto = JsonConvert.DeserializeObject<SimulationDto>(message);

        if (dto is not null)
            await receiver.HandleAsync(dto);
    }
}
