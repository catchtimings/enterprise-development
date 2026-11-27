using System.Text.Json;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FoodDelivery.Infrastructure.RabbitMq;

/// <summary>
/// Консьюмер RabbitMQ для приёма батчей контрактов заказов
/// </summary>
public class RabbitMqOrderConsumer(
    IConnection rabbitMqConnection,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<RabbitMqOrderConsumer> logger) : BackgroundService
{
    private readonly string _exchangeName =
        configuration.GetSection("RabbitMQ")["ExchangeName"]
        ?? throw new KeyNotFoundException("RabbitMQ:ExchangeName is missing");

    private readonly string _queueName =
        configuration.GetSection("RabbitMQ")["QueueName"]
        ?? throw new KeyNotFoundException("RabbitMQ:QueueName is missing");

    private readonly string _routingKey =
        configuration.GetSection("RabbitMQ")["RoutingKey"]
        ?? throw new KeyNotFoundException("RabbitMQ:RuttingKey is missing");

    private static readonly TimeSpan _retryDelay = TimeSpan.FromSeconds(5);

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Консьюмер запущен, очередь: {Queue}", _queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var channel = await rabbitMqConnection.CreateChannelAsync(
                    cancellationToken: stoppingToken);

                await DeclareTopologyAsync(channel, stoppingToken);

                var consumer = new OrderBatchConsumer(channel, scopeFactory, logger);
                await channel.BasicConsumeAsync(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);

                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
                return;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Консьюмер остановлен");
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "Очередь {Queue} недоступна, повтор через {Delay}с",
                    _queueName, _retryDelay.TotalSeconds);
                await Task.Delay(_retryDelay, stoppingToken);
            }
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

    /// <summary>
    /// Обработчик сообщений очереди
    /// </summary>
    private sealed class OrderBatchConsumer(
        IChannel channel,
        IServiceScopeFactory scopeFactory,
        ILogger logger) : AsyncDefaultBasicConsumer(channel)
    {
        /// <inheritdoc />
        public override async Task HandleBasicDeliverAsync(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IReadOnlyBasicProperties properties,
            ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var batch = JsonSerializer.Deserialize<IList<OrderCreateUpdateDto>>(
                    body.Span, _jsonOptions) ?? [];

                using var scope = scopeFactory.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<IOrderBatchReceiver>();
                var saved = await receiver.ReceiveBatch(batch, cancellationToken);

                logger.LogInformation(
                    "Принят батч из {Total} заказов, сохранено {Saved}, отсеяно {Rejected}",
                    batch.Count, saved, batch.Count - saved);

                await channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Ошибка обработки сообщения (routingKey={RoutingKey}); сообщение не возвращается в очередь",
                    routingKey);
                await channel.BasicNackAsync(deliveryTag, multiple: false, requeue: false, cancellationToken);
            }
        }
    }
}