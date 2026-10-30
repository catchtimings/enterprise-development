using FoodDelivery.Contracts.Dtos.Dishes;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности Dish в DTO и обратно
/// </summary>
public static class DishMapper
{
    public static DishDto Map(Dish entity)
        => new(entity.Id, entity.Name, entity.Weight, entity.Price, entity.CategoryId, entity.Category?.Name);

    public static Dish Map(DishCreateDto dto) => new()
    {
        Id = 0,
        Name = dto.Name,
        Weight = dto.Weight,
        Price = dto.Price,
        CategoryId = dto.CategoryId
    };

    public static List<DishDto> Map(List<Dish> entities) => [.. entities.Select(Map)];
}