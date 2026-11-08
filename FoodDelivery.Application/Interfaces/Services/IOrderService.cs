namespace FoodDelivery.Application.Interfaces.Services;

using FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// Определяет методы для управления заказами
/// </summary>
public interface IOrderService : IApplicationService<OrderDto, OrderCreateUpdateDto, int>
{
    /// <summary>
    /// Получение позиций в заказе
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <returns>Список позиций в заказе</returns>
    public Task<IList<OrderItemDto>> GetItems(int orderId);
}