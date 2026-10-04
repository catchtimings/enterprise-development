using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos;

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

    /// <summary>
    /// Фиксированная цена блюда на момент оформления заказа
    /// </summary>
    [Range(0, 1000000, ErrorMessage = "Цена должна быть неотрицательной")]
    public decimal PriceAtOrder { get; set; }
}