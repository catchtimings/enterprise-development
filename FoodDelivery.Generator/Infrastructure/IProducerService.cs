using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Generator.Infrastructure;

/// <summary>
/// Служба отправки сгенерированных контрактов по шине
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Отправляет батч контрактов заказов
    /// </summary>
    /// <param name="batch">Коллекция контрактов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task SendAsync(IList<OrderCreateUpdateDto> batch, CancellationToken cancellationToken);
}