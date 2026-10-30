using FoodDelivery.Contracts.Dtos.DishCategories;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущность DishCategory в DTO
/// </summary>
public static class DishCategoryMapper
{
    /// <summary>
    /// Преобразует сущность в DTO
    /// </summary>
    public static DishCategoryDto ToDto(DishCategory entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };
}