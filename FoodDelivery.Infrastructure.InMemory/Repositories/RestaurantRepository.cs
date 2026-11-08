using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий ресторанов в памяти
/// </summary>
public class RestaurantRepository(DataSeeder seeder) : IRestaurantRepository
{
    /// <inheritdoc />
    public Task<IList<Restaurant>> GetAll() => Task.FromResult<IList<Restaurant>>(seeder.Restaurants);

    /// <inheritdoc />
    public Task<Restaurant?> GetById(int id) => Task.FromResult(seeder.Restaurants.FirstOrDefault(r => r.Id == id));

    /// <inheritdoc />
    public Task<Restaurant> Add(Restaurant entity)
    {
        entity.Id = seeder.Restaurants.Any() ? seeder.Restaurants.Max(r => r.Id) + 1 : 1;
        seeder.Restaurants.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Restaurant> Update(Restaurant entity)
    {
        var existing = seeder.Restaurants.FirstOrDefault(r => r.Id == entity.Id);
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
        var existing = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (existing == null) return Task.FromResult(false);

        seeder.Restaurants.Remove(existing);
        return Task.FromResult(true);
    }
}