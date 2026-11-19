using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий ресторанов в памяти
/// </summary>
public class RestaurantInMemoryRepository : IRestaurantRepository
{
    private readonly List<Restaurant> _restaurants = [.. DataSeeder.Restaurants];

    /// <inheritdoc />
    public Task<IList<Restaurant>> GetAll() => Task.FromResult<IList<Restaurant>>(_restaurants);

    /// <inheritdoc />
    public Task<Restaurant?> GetById(int id) => Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));

    /// <inheritdoc />
    public Task<Restaurant> Add(Restaurant entity)
    {
        entity.Id = _restaurants.Any() ? _restaurants.Max(r => r.Id) + 1 : 1;
        _restaurants.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Restaurant> Update(Restaurant entity)
    {
        var existing = _restaurants.FirstOrDefault(r => r.Id == entity.Id);
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
        var existing = _restaurants.FirstOrDefault(r => r.Id == id);
        if (existing == null) return Task.FromResult(false);

        _restaurants.Remove(existing);
        return Task.FromResult(true);
    }
}