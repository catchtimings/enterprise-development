using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Позиция в заказе
/// </summary>
[Table("order_items")]
public class OrderItem
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    [Column("order_id")]
    public required int OrderId { get; set; }

    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    [Column("dish_id")]
    public required int DishId { get; set; }

    /// <summary>
    /// Блюдо
    /// </summary>
    public Dish? Dish { get; set; }

    /// <summary>
    /// Количество порций
    /// </summary>
    [Column("quantity")]
    public required int Quantity { get; set; }

    /// <summary>
    /// Фиксированная цена блюда на момент оформления заказа
    /// </summary>
    [Column("price_at_order")]
    public decimal PriceAtOrder { get; set; }
}