using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Restaurants;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления ресторанами
/// </summary>
public class RestaurantService(IRestaurantRepository restaurantRepository) : IRestaurantService
{
    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    public async Task<RestaurantDto> Create(RestaurantCreateUpdateDto dto)
    {
        var restaurant = RestaurantMapper.Map(dto);
        var createdRestaurant = await restaurantRepository.Add(restaurant);
        return RestaurantMapper.Map(createdRestaurant);
    }

    /// <summary>
    /// Возвращает ресторан по идентификатору
    /// </summary>
    public async Task<RestaurantDto?> Get(int id)
    {
        var restaurant = await restaurantRepository.GetById(id);
        return restaurant is null ? null : RestaurantMapper.Map(restaurant);
    }

    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    public async Task<IList<RestaurantDto>> GetAll()
    {
        var restaurants = await restaurantRepository.GetAll();
        return RestaurantMapper.Map(restaurants.ToList());
    }

    /// <summary>
    /// Обновляет данные ресторана
    /// </summary>
    public async Task<RestaurantDto> Update(int id, RestaurantCreateUpdateDto dto)
    {
        var existing = await restaurantRepository.GetById(id)
            ?? throw new KeyNotFoundException("Ресторан не найден");

        existing.Name = dto.Name;
        existing.Address = dto.Address;
        existing.Rating = dto.Rating;
        existing.OpeningTime = dto.OpeningTime;
        existing.ClosingTime = dto.ClosingTime;

        var updatedRestaurant = await restaurantRepository.Update(existing);
        return RestaurantMapper.Map(updatedRestaurant);
    }

    /// <summary>
    /// Удаляет ресторан
    /// </summary>
    public Task<bool> Delete(int id) => restaurantRepository.Delete(id);
}