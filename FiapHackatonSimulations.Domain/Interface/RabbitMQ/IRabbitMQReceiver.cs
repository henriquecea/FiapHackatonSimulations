using FiapHackatonSimulations.Domain.DTO;

namespace FiapHackatonSimulations.Domain.Interface.RabbitMQ;

public interface IRabbitMQReceiver
{
    Task HandleAsync(SimulationDto message);
}
