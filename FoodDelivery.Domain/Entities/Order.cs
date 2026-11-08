using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Заказ клиента
/// </summary>
[Table("orders")]
public class Order
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Column("client_id")]
    public required int ClientId { get; set; }

    /// <summary>
    /// Клиент, оформивший заказ
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    [Column("restaurant_id")]
    public required int RestaurantId { get; set; }

    /// <summary>
    /// Ресторан, исполняющий заказ
    /// </summary>
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Дата и время оформления заказа
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    [Column("delivered_at")]
    public DateTime? DeliveredAt { get; set; }

    /// <summary>
    /// Итоговая сумма заказа
    /// </summary>
    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Список позиций блюд в заказе
    /// </summary>
    public List<OrderItem> Items { get; set; } = [];

    /// <summary>
    /// Время, затраченное на доставку
    /// </summary>
    public TimeSpan? DeliveryDuration => DeliveredAt - CreatedAt;
}