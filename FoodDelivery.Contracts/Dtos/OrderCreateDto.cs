using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos;

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
    /// Дата и время оформления заказа
    /// </summary>
    [Required(ErrorMessage = "Дата оформления обязательна")]
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время доставки
    /// </summary>
    [Required(ErrorMessage = "Дата доставки обязательна")]
    public required DateTime DeliveredAt { get; set; }

    /// <summary>
    /// Итоговая сумма заказа
    /// </summary>
    [Range(0, 100000000, ErrorMessage = "Сумма заказа должна быть неотрицательной")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Список позиций заказа
    /// </summary>
    public List<OrderItemCreateDto> Items { get; set; } = [];
}