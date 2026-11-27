using FoodDelivery.Generator.Domain;
using FoodDelivery.Generator.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.Generator.Application;

/// <summary>
/// Фоновая служба автоматической генерации и отправки заказов
/// </summary>
public class OrderGeneratorService(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<OrderGeneratorService> logger) : BackgroundService
{
    private readonly int _batchSize = configuration.GetValue("Generator:BatchSize", 5);
    private readonly int _payloadLimit = configuration.GetValue("Generator:PayloadLimit", 20);
    private readonly int _waitTimeSeconds = configuration.GetValue("Generator:WaitTimeSeconds", 3);

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Генератор запущен: batchSize={BatchSize}, payloadLimit={PayloadLimit}, waitTime={WaitTime}s",
            _batchSize, _payloadLimit, _waitTimeSeconds);

        if (_batchSize <= 0 || _payloadLimit <= 0 || _waitTimeSeconds < 0)
            throw new FormatException("Параметры Generator должны быть положительными");

        var sent = 0;
        using var scope = scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();
        var generator = scope.ServiceProvider.GetRequiredService<BogusOrderGenerator>();

        while (sent < _payloadLimit && !cancellationToken.IsCancellationRequested)
        {
            var batch = generator.GenerateBatch(Math.Min(_batchSize, _payloadLimit - sent));
            await producer.SendAsync(batch, cancellationToken);
            sent += batch.Count;

            logger.LogInformation("Отправлено {Sent}/{Limit} заказов", sent, _payloadLimit);

            if (sent < _payloadLimit)
                await Task.Delay(TimeSpan.FromSeconds(_waitTimeSeconds), cancellationToken);
        }

        logger.LogInformation("Генератор завершил работу: отправлено {Sent} заказов", sent);
    }
}