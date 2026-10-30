using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// Заказы с минимальным временем доставки
/// </summary>
/// <param name="MinDeliveryTime">Минимальное время доставки</param>
/// <param name="Orders">Список заказов с минимальным временем доставки</param>
public record MinDeliveryTimeOrdersDto(TimeSpan MinDeliveryTime, List<OrderDto> Orders);