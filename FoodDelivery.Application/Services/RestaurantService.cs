using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Restaurants;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления ресторанами
/// </summary>
public class RestaurantService(IRepository<Restaurant> restaurantRepository)
{
    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    public async Task<RestaurantDto> Create(RestaurantCreateDto dto)
    {
        var restaurant = RestaurantMapper.ToEntity(dto);
        await restaurantRepository.Add(restaurant);
        return RestaurantMapper.ToDto(restaurant);
    }

    /// <summary>
    /// Возвращает ресторан по идентификатору
    /// </summary>
    public async Task<RestaurantDto?> Get(int id)
    {
        var restaurant = await restaurantRepository.GetById(id);
        return restaurant is null ? null : RestaurantMapper.ToDto(restaurant);
    }

    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    public async Task<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurants = await restaurantRepository.GetAll();
        return restaurants.Select(RestaurantMapper.ToDto);
    }

    /// <summary>
    /// Обновляет данные ресторана
    /// </summary>
    public async Task<RestaurantDto> Update(int id, RestaurantCreateDto dto)
    {
        var existing = await restaurantRepository.GetById(id)
            ?? throw new KeyNotFoundException("Ресторан не найден");

        existing.Name = dto.Name;
        existing.Address = dto.Address;
        existing.Rating = dto.Rating;
        existing.OpeningTime = dto.OpeningTime;
        existing.ClosingTime = dto.ClosingTime;

        await restaurantRepository.Update(existing);
        return RestaurantMapper.ToDto(existing);
    }

    /// <summary>
    /// Удаляет ресторан
    /// </summary>
    public Task Delete(int id) => restaurantRepository.Delete(id);
}