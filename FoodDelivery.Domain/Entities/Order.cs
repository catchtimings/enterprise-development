namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Заказ клиента
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// Клиент, оформивший заказ
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public required int RestaurantId { get; set; }

    /// <summary>
    /// Ресторан, исполняющий заказ
    /// </summary>
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Дата и время оформления заказа
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    public DateTimeOffset DeliveredAt { get; set; }

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