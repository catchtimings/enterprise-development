using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO для создания позиции заказа
/// </summary>
/// <param name="DishId">Идентификатор блюда</param>
/// <param name="Quantity">Количество порций</param>
public record OrderItemCreateDto(
    [property: Required(ErrorMessage = "Идентификатор блюда обязателен")]
    int DishId,
    [property: Range(1, 1000, ErrorMessage = "Количество должно быть положительным")]
    int Quantity
);