using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Generator.Infrastructure;
using RabbitMQ.Client;
using System.Text.Json;

namespace FoodDelivery.Generator.Rabbitmq.Infrastructure;

/// <summary>
/// Продюсер RabbitMQ для отправки батчей контрактов заказов
/// </summary>
public class RabbitMqProducer(
    IConnection rabbitMqConnection,
    IConfiguration configuration,
    ILogger<RabbitMqProducer> logger) : IProducerService
{
    private readonly string _exchangeName =
        configuration.GetSection("RabbitMQ")["ExchangeName"]
        ?? throw new KeyNotFoundException("RabbitMQ:ExchangeName is missing");

    private readonly string _queueName =
        configuration.GetSection("RabbitMQ")["QueueName"]
        ?? throw new KeyNotFoundException("RabbitMQ:QueueName is missing");

    private readonly string _routingKey =
        configuration.GetSection("RabbitMQ")["RoutingKey"]
        ?? throw new KeyNotFoundException("RabbitMQ:RoutingKey is missing");

    /// <inheritdoc />
    public async Task SendAsync(IList<OrderCreateUpdateDto> batch, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Отправка батча из {Count} заказов в exchange {Exchange}",
                batch.Count, _exchangeName);

            await using var channel = await rabbitMqConnection.CreateChannelAsync(
                cancellationToken: cancellationToken);

            await DeclareTopologyAsync(channel, cancellationToken);

            var payload = JsonSerializer.SerializeToUtf8Bytes(batch.ToList());
            await channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: _routingKey,
                mandatory: false,
                basicProperties: new BasicProperties { DeliveryMode = DeliveryModes.Persistent },
                body: payload,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Батч из {Count} заказов отправлен (routingKey={RoutingKey})",
                batch.Count, _routingKey);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Отправка батча отменена");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка отправки батча из {Count} заказов", batch.Count);
            throw;
        }
    }

    /// <summary>
    /// Объявляет топологию: topic exchange, durable queue и binding
    /// </summary>
    /// <param name="channel">Канал брокера</param>
    /// <param name="cancellationToken">Токен отмены</param>
    private async Task DeclareTopologyAsync(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: _queueName,
            exchange: _exchangeName,
            routingKey: _routingKey,
            cancellationToken: cancellationToken);
    }
}