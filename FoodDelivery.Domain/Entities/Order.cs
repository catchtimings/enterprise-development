namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Заказ клиента
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Клиент, оформивший заказ
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public Guid RestaurantId { get; set; }

    /// <summary>
    /// Ресторан, исполняющий заказ
    /// </summary>
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Дата и время оформления заказа
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    public DateTime DeliveredAt { get; set; }

    /// <summary>
    /// Итоговая сумма заказа
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Список позиций блюд в заказе
    /// </summary>
    public List<OrderItem> Items { get; set; } = [];

    /// <summary>
    /// Время, затраченное на доставку
    /// </summary>
    public TimeSpan DeliveryDuration => DeliveredAt - CreatedAt;
}