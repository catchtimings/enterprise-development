namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO позиции заказа
/// </summary>
/// <param name="Id">Уникальный идентификатор позиции заказа</param>
/// <param name="OrderId">Идентификатор заказа</param>
/// <param name="DishId">Идентификатор блюда</param>
/// <param name="DishName">Название блюда</param>
/// <param name="Quantity">Количество единиц блюда</param>
/// <param name="PriceAtOrder">Фиксированная цена блюда на момент оформления заказа</param>
public record OrderItemDto(int Id, int OrderId, int DishId, string? DishName, int Quantity, decimal PriceAtOrder);