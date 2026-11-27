using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Generator.Domain;
using FoodDelivery.Generator.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.Generator.Presentation;

/// <summary>
/// Контроллер запуска генерации заказов
/// </summary>
/// <param name="producer">Служба отправки</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/generator")]
public class OrdersGeneratorController(
    IProducerService producer,
    ILogger<OrdersGeneratorController> logger) : ControllerBase
{
    /// <summary>
    /// Генерирует и отправляет заданное число контрактов заказов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<OrderCreateUpdateDto>>> Generate(
        [FromQuery] int batchSize = 5,
        [FromQuery] int payloadLimit = 20,
        [FromQuery] int waitTime = 2,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0 || payloadLimit <= 0 || waitTime < 0)
            return BadRequest("batchSize и payloadLimit должны быть > 0, waitTime >= 0");

        try
        {
            logger.LogInformation(
                "Ручная генерация: batchSize={BatchSize}, payloadLimit={PayloadLimit}, waitTime={WaitTime}s",
                batchSize, payloadLimit, waitTime);

            var all = new List<OrderCreateUpdateDto>(payloadLimit);
            var sent = 0;

            while (sent < payloadLimit && !cancellationToken.IsCancellationRequested)
            {
                var batch = OrderGenerator.GenerateBatch(Math.Min(batchSize, payloadLimit - sent));
                await producer.SendAsync(batch, cancellationToken);
                sent += batch.Count;
                all.AddRange(batch);

                if (sent < payloadLimit)
                    await Task.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken);
            }

            logger.LogInformation("Ручная генерация завершена: отправлено {Sent} заказов", sent);
            return Ok(all);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка ручной генерации");
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}