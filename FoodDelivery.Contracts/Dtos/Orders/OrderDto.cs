namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO заказа клиента
/// </summary>
/// <param name="Id">Идентификатор заказа</param>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="ClientName">ФИО клиента, оформившего заказ</param>
/// <param name="RestaurantId">Идентификатор ресторана</param>
/// <param name="RestaurantName">Название ресторана, исполняющего заказ</param>
/// <param name="CreatedAt">Дата и время оформления заказа</param>
/// <param name="DeliveredAt">Дата и время доставки</param>
/// <param name="TotalAmount">Итоговая сумма заказа в рублях</param>
/// <param name="DeliveryDuration">Время, затраченное на доставку</param>
/// <param name="Items">Список позиций заказа</param>
public record OrderDto(
    int Id,
    int ClientId,
    string? ClientName,
    int RestaurantId,
    string? RestaurantName,
    DateTime CreatedAt,
    DateTime DeliveredAt,
    decimal TotalAmount,
    TimeSpan DeliveryDuration,
    List<OrderItemDto> Items
);