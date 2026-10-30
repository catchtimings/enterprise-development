namespace FoodDelivery.Contracts.Dtos.DishCategories;

/// <summary>
/// DTO категории блюда
/// </summary>
/// <param name="Id">Идентификатор категории</param>
/// <param name="Name">Название категории</param>
public record DishCategoryDto(int Id, string Name);