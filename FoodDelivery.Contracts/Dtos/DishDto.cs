namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO блюда ресторана
/// </summary>
public class DishDto
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название блюда
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Вес блюда в граммах
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// Цена блюда в рублях
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Идентификатор категории блюда
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Название категории блюда
    /// </summary>
    public string? CategoryName { get; set; }
}