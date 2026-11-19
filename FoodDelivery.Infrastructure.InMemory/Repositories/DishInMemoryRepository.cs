using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий блюд в памяти
/// </summary>
public class DishInMemoryRepository : IDishRepository
{
    private readonly List<Dish> _dishes = [.. DataSeeder.Dishes];

    /// <inheritdoc />
    public Task<IList<Dish>> GetAll() => Task.FromResult<IList<Dish>>(_dishes);

    /// <inheritdoc />
    public Task<Dish?> GetById(int id) => Task.FromResult(_dishes.FirstOrDefault(d => d.Id == id));

    /// <inheritdoc />
    public Task<Dish> Add(Dish entity)
    {
        entity.Id = _dishes.Any() ? _dishes.Max(d => d.Id) + 1 : 1;
        _dishes.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Dish> Update(Dish entity)
    {
        var existing = _dishes.FirstOrDefault(d => d.Id == entity.Id);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Weight = entity.Weight;
            existing.Price = entity.Price;
            existing.CategoryId = entity.CategoryId;
            existing.Category = entity.Category;
        }
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = _dishes.FirstOrDefault(d => d.Id == id);
        if (existing == null) return Task.FromResult(false);

        _dishes.Remove(existing);
        return Task.FromResult(true);
    }
}