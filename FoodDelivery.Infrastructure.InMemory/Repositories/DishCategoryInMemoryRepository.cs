using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий категорий блюд в памяти
/// </summary>
public class DishCategoryInMemoryRepository : IDishCategoryRepository
{
    private readonly List<DishCategory> _categories = [.. DataSeeder.Categories];

    /// <inheritdoc />
    public Task<IList<DishCategory>> GetAll() => Task.FromResult<IList<DishCategory>>(_categories);

    /// <inheritdoc />
    public Task<DishCategory?> GetById(int id) => Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));

    /// <inheritdoc />
    public Task<DishCategory> Add(DishCategory entity)
    {
        entity.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
        _categories.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<DishCategory> Update(DishCategory entity)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null)
        {
            existing.Name = entity.Name;
        }
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult(false);

        _categories.Remove(existing);
        return Task.FromResult(true);
    }
}