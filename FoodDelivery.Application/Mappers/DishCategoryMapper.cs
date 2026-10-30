using FoodDelivery.Contracts.Dtos.DishCategories;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности DishCategory в DTO
/// </summary>
public static class DishCategoryMapper
{
    public static DishCategoryDto Map(DishCategory entity) => new(entity.Id, entity.Name);

    public static DishCategory Map(DishCategoryCreateUpdateDto dto) => new()
    {
        Id = 0,
        Name = dto.Name
    };
    public static List<DishCategoryDto> Map(List<DishCategory> entities) => [.. entities.Select(Map)];
}