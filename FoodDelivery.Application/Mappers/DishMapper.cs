using FoodDelivery.Contracts.Dtos.Dishes;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущность Dish в DTO и обратно
/// </summary>
public static class DishMapper
{
    /// <summary>
    /// Преобразует сущность в DTO
    /// </summary>
    public static DishDto ToDto(Dish entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Weight = entity.Weight,
        Price = entity.Price,
        CategoryId = entity.CategoryId,
        CategoryName = entity.Category?.Name
    };

    /// <summary>
    /// Создает базовую сущность из DTO
    /// </summary>
    public static Dish ToEntity(DishCreateDto dto) => new()
    {
        Id = 0,
        Name = dto.Name,
        Weight = dto.Weight,
        Price = dto.Price,
        CategoryId = dto.CategoryId
    };
}