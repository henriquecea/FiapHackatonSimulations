using FiapHackatonSimulations.Domain.Interface.Service;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FiapHackatonSimulations.Application.Service.RabbitMQ;

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQService(IConnectionFactory factory)
    {
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string queue, T message)
    {
        await _channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queue,
            body: body);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
