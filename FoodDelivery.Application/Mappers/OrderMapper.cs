using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности Order и OrderItem в DTO и обратно
/// </summary>
public static class OrderMapper
{
    /// <summary>
    /// Преобразует позицию заказа в DTO
    /// </summary>
    public static OrderItemDto ToDto(OrderItem entity) => new()
    {
        Id = entity.Id,
        OrderId = entity.OrderId,
        DishId = entity.DishId,
        DishName = entity.Dish?.Name,
        Quantity = entity.Quantity,
        PriceAtOrder = entity.PriceAtOrder
    };

    /// <summary>
    /// Преобразует заказ в DTO
    /// </summary>
    public static OrderDto ToDto(Order entity) => new()
    {
        Id = entity.Id,
        ClientId = entity.ClientId,
        ClientName = entity.Client?.FullName,
        RestaurantId = entity.RestaurantId,
        RestaurantName = entity.Restaurant?.Name,
        CreatedAt = entity.CreatedAt,
        DeliveredAt = entity.DeliveredAt,
        TotalAmount = entity.TotalAmount,
        DeliveryDuration = entity.DeliveryDuration,
        Items = entity.Items.Select(ToDto).ToList()
    };

    /// <summary>
    /// Создает позицию заказа из DTO
    /// </summary>
    public static OrderItem ToEntity(OrderItemCreateDto dto) => new()
    {
        Id = 0,
        OrderId = 0,
        DishId = dto.DishId,
        Quantity = dto.Quantity,
        PriceAtOrder = 0
    };
}