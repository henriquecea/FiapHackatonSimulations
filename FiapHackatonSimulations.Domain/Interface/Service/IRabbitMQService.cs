namespace FiapHackatonSimulations.Domain.Interface.Service;

public interface IRabbitMQService
{
    Task PublishAsync<T>(string queue, T message);
}
