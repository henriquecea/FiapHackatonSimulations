using FiapHackatonSimulations.Domain.Interface.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FiapHackatonSimulations.Application.Service.RabbitMQ;

public class RabbitMQConsumer(
    IConnectionFactory factory,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                break; // conexão estabelecida, sai do loop
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ ainda não disponível. Tentando novamente em 5s...");
                await Task.Delay(5000, stoppingToken);
            }
        }

        var scope = scopeFactory.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IRabbitMQMessageHandler>();

        foreach (var handler in handlers)
        {
            await _channel.QueueDeclareAsync(
                queue: handler.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                using var messageScope = scopeFactory.CreateScope();
                var scopedHandler = messageScope.ServiceProvider
                    .GetServices<IRabbitMQMessageHandler>()
                    .First(h => h.QueueName == handler.QueueName);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    await scopedHandler.HandleAsync(message);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: handler.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }
    }
}
