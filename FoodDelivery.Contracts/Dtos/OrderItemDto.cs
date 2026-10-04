namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO позиции заказа
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public int DishId { get; set; }

    /// <summary>
    /// Название блюда
    /// </summary>
    public string? DishName { get; set; }

    /// <summary>
    /// Количество порций
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Фиксированная цена блюда на момент оформления заказа
    /// </summary>
    public decimal PriceAtOrder { get; set; }
}