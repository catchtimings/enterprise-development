using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO для создания или обновления заказа
/// </summary>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="RestaurantId">Идентификатор ресторана</param>
/// <param name="DeliveredAt">Дата и время доставки</param>
/// <param name="Items">Список позиций заказа</param>
public record OrderCreateDto(
    [property: Required(ErrorMessage = "Идентификатор клиента обязателен")]
    int ClientId,
    [property: Required(ErrorMessage = "Идентификатор ресторана обязателен")]
    int RestaurantId,
    DateTime DeliveredAt,
    List<OrderItemCreateDto> Items
);