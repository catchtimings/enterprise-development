using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO для создания позиции заказа
/// </summary>
public class OrderItemCreateDto
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    [Required(ErrorMessage = "Идентификатор блюда обязателен")]
    public required int DishId { get; set; }

    /// <summary>
    /// Количество порций
    /// </summary>
    [Range(1, 1000, ErrorMessage = "Количество должно быть положительным")]
    public required int Quantity { get; set; }

}