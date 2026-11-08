using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий категорий блюд в памяти
/// </summary>
public class DishCategoryRepository : IDishCategoryRepository
{
    /// <inheritdoc />
    public Task<IList<DishCategory>> GetAll() => Task.FromResult<IList<DishCategory>>(DataSeeder.Categories);

    /// <inheritdoc />
    public Task<DishCategory?> GetById(int id) => Task.FromResult(DataSeeder.Categories.FirstOrDefault(c => c.Id == id));

    /// <inheritdoc />
    public Task<DishCategory> Add(DishCategory entity)
    {
        entity.Id = DataSeeder.Categories.Any() ? DataSeeder.Categories.Max(c => c.Id) + 1 : 1;
        DataSeeder.Categories.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<DishCategory> Update(DishCategory entity)
    {
        var existing = DataSeeder.Categories.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null) existing.Name = entity.Name;
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = DataSeeder.Categories.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult(false);

        DataSeeder.Categories.Remove(existing);
        return Task.FromResult(true);
    }
}