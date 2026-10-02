namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO категории блюда
/// </summary>
public class DishCategoryDto
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название категории
    /// </summary>
    public required string Name { get; set; }
}