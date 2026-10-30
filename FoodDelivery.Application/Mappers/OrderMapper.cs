using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности Order и OrderItem в DTO и обратно
/// </summary>
public static class OrderMapper
{
    public static OrderItemDto Map(OrderItem entity)
        => new(entity.Id, entity.OrderId, entity.DishId, entity.Dish?.Name, entity.Quantity, entity.PriceAtOrder);

    public static OrderDto Map(Order entity) => new(
        entity.Id,
        entity.ClientId,
        entity.Client?.FullName,
        entity.RestaurantId,
        entity.Restaurant?.Name,
        entity.CreatedAt,
        entity.DeliveredAt,
        entity.TotalAmount,
        entity.DeliveryDuration,
        entity.Items.Select(Map).ToList()
    );

    public static OrderItem Map(OrderItemCreateUpdateDto dto) => new()
    {
        Id = 0,
        OrderId = 0,
        DishId = dto.DishId,
        Quantity = dto.Quantity,
        PriceAtOrder = 0
    };

    public static List<OrderDto> Map(List<Order> entities) => [.. entities.Select(Map)];
}