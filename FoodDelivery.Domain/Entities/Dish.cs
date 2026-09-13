namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Блюдо ресторана
/// </summary>
public class Dish
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название блюда
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Вес блюда в граммах
    /// </summary>
    public int WeightInGrams { get; set; }

    /// <summary>
    /// Цена блюда в рублях
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Идентификатор категории блюда
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Категория блюда
    /// </summary>
    public DishCategory? Category { get; set; }
}