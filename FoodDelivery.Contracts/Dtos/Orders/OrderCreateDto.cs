using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO для создания или обновления заказа
/// </summary>
public class OrderCreateDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Required(ErrorMessage = "Идентификатор клиента обязателен")]
    public required int ClientId { get; set; }

    /// <summary>
    /// Идентификатор ресторана
    /// </summary>
    [Required(ErrorMessage = "Идентификатор ресторана обязателен")]
    public required int RestaurantId { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    [Required(ErrorMessage = "Дата доставки обязательна")]
    public required DateTime DeliveredAt { get; set; }

    /// <summary>
    /// Список позиций заказа
    /// </summary>
    public List<OrderItemCreateDto> Items { get; set; } = [];
}