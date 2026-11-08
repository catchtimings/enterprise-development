using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий блюд в базе данных
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class DishEfCoreRepository(FoodDeliveryDbContext dbContext) : IDishRepository
{
    /// <inheritdoc />
    public async Task<Dish> Add(Dish entity)
    {
        var result = await dbContext.Dishes.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var entity = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (entity == null)
            return false;

        dbContext.Dishes.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<IList<Dish>> GetAll() =>
        await dbContext.Dishes
            .Include(d => d.Category)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<Dish?> GetById(int id) =>
        await dbContext.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);

    /// <inheritdoc />
    public async Task<Dish> Update(Dish entity)
    {
        var result = dbContext.Dishes.Update(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}