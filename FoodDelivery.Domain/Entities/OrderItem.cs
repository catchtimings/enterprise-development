namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Позиция в заказе
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public Guid DishId { get; set; }

    /// <summary>
    /// Блюдо
    /// </summary>
    public Dish? Dish { get; set; }

    /// <summary>
    /// Количество порций
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Фиксированная цена блюда на момент оформления заказа
    /// </summary>
    public decimal PriceAtOrder { get; set; }
}