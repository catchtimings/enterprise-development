using FoodDelivery.Application.Interfaces;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий категорий блюд в памяти
/// </summary>
public class DishCategoryRepository(DataSeeder seeder) : IRepository<DishCategory>
{
    /// <inheritdoc />
    public Task<IList<DishCategory>> GetAll() => Task.FromResult<IList<DishCategory>>(seeder.Categories);

    /// <inheritdoc />
    public Task<DishCategory?> GetById(int id) => Task.FromResult(seeder.Categories.FirstOrDefault(c => c.Id == id));

    /// <inheritdoc />
    public Task<DishCategory> Add(DishCategory entity)
    {
        entity.Id = seeder.Categories.Any() ? seeder.Categories.Max(c => c.Id) + 1 : 1;
        seeder.Categories.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<DishCategory> Update(DishCategory entity)
    {
        var existing = seeder.Categories.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null) existing.Name = entity.Name;
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = seeder.Categories.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult(false);

        seeder.Categories.Remove(existing);
        return Task.FromResult(true);
    }
}