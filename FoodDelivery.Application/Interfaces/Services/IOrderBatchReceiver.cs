using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Application.Interfaces.Services;

/// <summary>
/// Приём пакетов контрактов заказов из шины сообщений
/// </summary>
public interface IOrderBatchReceiver
{
    /// <summary>
    /// Принимает пакет контрактов и сохраняет валидные заказы
    /// </summary>
    /// <param name="batch">Пакет контрактов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Количество успешно сохранённых заказов</returns>
    public Task<int> ReceiveBatch(IList<OrderCreateUpdateDto> batch, CancellationToken cancellationToken);
}