namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Категория блюда
/// </summary>
public class DishCategory
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название категории
    /// </summary>
    public required string Name { get; set; }
}