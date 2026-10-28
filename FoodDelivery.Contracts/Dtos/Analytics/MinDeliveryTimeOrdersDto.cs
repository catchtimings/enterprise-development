using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// DTO заказов с минимальным временем доставки
/// </summary>
public class MinDeliveryTimeOrdersDto
{
    /// <summary>
    /// Минимальное время доставки
    /// </summary>
    public TimeSpan MinDeliveryTime { get; set; }

    /// <summary>
    /// Заказы с минимальным временем доставки
    /// </summary>
    public required List<OrderDto> Orders { get; set; }
}