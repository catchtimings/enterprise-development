using FoodDelivery.Contracts.Dtos.Restaurants;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности Restaurant в DTO и обратно
/// </summary>
public static class RestaurantMapper
{
    public static RestaurantDto Map(Restaurant entity)
        => new(entity.Id, entity.Name, entity.Address, entity.Rating, entity.OpeningTime, entity.ClosingTime);

    public static Restaurant Map(RestaurantCreateDto dto) => new()
    {
        Id = 0,
        Name = dto.Name,
        Address = dto.Address,
        Rating = dto.Rating,
        OpeningTime = dto.OpeningTime,
        ClosingTime = dto.ClosingTime
    };

    public static List<RestaurantDto> Map(List<Restaurant> entities) => [.. entities.Select(Map)];
}