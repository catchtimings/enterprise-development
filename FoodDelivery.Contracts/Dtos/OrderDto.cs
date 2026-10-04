namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO заказа клиента
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// ФИО клиента, оформившего заказ
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// Название ресторана, исполняющего заказ
    /// </summary>
    public string? RestaurantName { get; set; }

    /// <summary>
    /// Дата и время оформления заказа
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    public DateTime DeliveredAt { get; set; }

    /// <summary>
    /// Итоговая сумма заказа в рублях
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Время, затраченное на доставку
    /// </summary>
    public TimeSpan DeliveryDuration { get; set; }

    /// <summary>
    /// Список позиций заказа
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = [];
}