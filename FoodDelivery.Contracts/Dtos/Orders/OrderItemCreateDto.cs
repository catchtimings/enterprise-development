namespace FoodDelivery.Contracts.Dtos.Orders;

/// <summary>
/// DTO для создания позиции заказа
/// </summary>
/// <param name="DishId">Идентификатор блюда</param>
/// <param name="Quantity">Количество порций</param>
public record OrderItemCreateDto(int DishId, int Quantity);