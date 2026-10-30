using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// Позиция заказа. Цена берётся сервером из блюда
/// </summary>
/// <param name="DishId">Идентификатор блюда</param>
/// <param name="Quantity">Количество порций</param>
public record OrderItemCreateUpdateDto(
    [Range(1, int.MaxValue)] int DishId,
    [Range(1, 1000)] int Quantity
);