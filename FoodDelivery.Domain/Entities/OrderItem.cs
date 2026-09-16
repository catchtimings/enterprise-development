namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Позиция в заказе
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public required int OrderId { get; set; }

    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public required int DishId { get; set; }

    /// <summary>
    /// Блюдо
    /// </summary>
    public Dish? Dish { get; set; }

    /// <summary>
    /// Количество порций
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Фиксированная цена блюда на момент оформления заказа
    /// </summary>
    public decimal PriceAtOrder { get; set; }
}