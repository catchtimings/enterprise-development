using FoodDelivery.Contracts.Dtos.DishCategories;

namespace FoodDelivery.Application.Interfaces.Services;

/// <summary>
/// Определяет методы для управления категориями блюд
/// </summary>
public interface IDishCategoryService : IApplicationService<DishCategoryDto, DishCategoryCreateUpdateDto, int>;