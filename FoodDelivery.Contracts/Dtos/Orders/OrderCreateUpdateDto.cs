using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// Данные для создания и обновления заказа.
/// Дату оформления, цены позиций и итоговую сумму определяет сервер
/// </summary>
/// <param name="ClientId">Идентификатор клиента</param>
/// <param name="RestaurantId">Идентификатор ресторана</param>
/// <param name="DeliveredAt">Время доставки (плановое); должно быть позже времени оформления заказа</param>
/// <param name="Items">Позиции заказа, минимум одна</param>
public record OrderCreateUpdateDto(
    [Range(1, int.MaxValue)] int ClientId,
    [Range(1, int.MaxValue)] int RestaurantId,
    DateTime? DeliveredAt,
    [Required, MinLength(1, ErrorMessage = "Заказ должен содержать хотя бы одну позицию")] List<OrderItemCreateUpdateDto> Items
);