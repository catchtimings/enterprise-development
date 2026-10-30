using FoodDelivery.Contracts.Dtos.Restaurants;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущность Restaurant в DTO и обратно
/// </summary>
public static class RestaurantMapper
{
    /// <summary>
    /// Преобразует сущность в DTO
    /// </summary>
    public static RestaurantDto ToDto(Restaurant entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Address = entity.Address,
        Rating = entity.Rating,
        OpeningTime = entity.OpeningTime,
        ClosingTime = entity.ClosingTime
    };

    /// <summary>
    /// Создает сущность из DTO
    /// </summary>
    public static Restaurant ToEntity(RestaurantCreateDto dto) => new()
    {
        Id = 0,
        Name = dto.Name,
        Address = dto.Address,
        Rating = dto.Rating,
        OpeningTime = dto.OpeningTime,
        ClosingTime = dto.ClosingTime
    };
}