using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий ресторанов в памяти
/// </summary>
public class RestaurantRepository : IRestaurantRepository
{
    /// <inheritdoc />
    public Task<IList<Restaurant>> GetAll() => Task.FromResult<IList<Restaurant>>(DataSeeder.Restaurants);

    /// <inheritdoc />
    public Task<Restaurant?> GetById(int id) => Task.FromResult(DataSeeder.Restaurants.FirstOrDefault(r => r.Id == id));

    /// <inheritdoc />
    public Task<Restaurant> Add(Restaurant entity)
    {
        entity.Id = DataSeeder.Restaurants.Any() ? DataSeeder.Restaurants.Max(r => r.Id) + 1 : 1;
        DataSeeder.Restaurants.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Restaurant> Update(Restaurant entity)
    {
        var existing = DataSeeder.Restaurants.FirstOrDefault(r => r.Id == entity.Id);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Address = entity.Address;
            existing.Rating = entity.Rating;
            existing.OpeningTime = entity.OpeningTime;
            existing.ClosingTime = entity.ClosingTime;
        }
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = DataSeeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (existing == null) return Task.FromResult(false);

        DataSeeder.Restaurants.Remove(existing);
        return Task.FromResult(true);
    }
}