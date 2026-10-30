using FoodDelivery.Application.Interfaces;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий ресторанов в памяти
/// </summary>
public class RestaurantRepository(DataSeeder seeder) : IRepository<Restaurant>
{
    /// <inheritdoc />
    public Task<IEnumerable<Restaurant>> GetAll() => Task.FromResult<IEnumerable<Restaurant>>(seeder.Restaurants);

    /// <inheritdoc />
    public Task<Restaurant?> GetById(int id) => Task.FromResult(seeder.Restaurants.FirstOrDefault(r => r.Id == id));

    /// <inheritdoc />
    public Task Add(Restaurant entity)
    {
        entity.Id = seeder.Restaurants.Any() ? seeder.Restaurants.Max(r => r.Id) + 1 : 1;
        seeder.Restaurants.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task Update(Restaurant entity)
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
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task Delete(int id)
    {
        var existing = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (existing != null) seeder.Restaurants.Remove(existing);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> IsExists(int id) => Task.FromResult(seeder.Restaurants.Any(r => r.Id == id));
}