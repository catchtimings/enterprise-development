using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий категорий блюд в базе данных
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class DishCategoryEfCoreRepository(FoodDeliveryDbContext dbContext) : IDishCategoryRepository
{
    /// <inheritdoc />
    public async Task<DishCategory> Add(DishCategory entity)
    {
        var result = await dbContext.DishCategories.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var entity = await dbContext.DishCategories.FirstOrDefaultAsync(c => c.Id == id);
        if (entity == null)
            return false;

        dbContext.DishCategories.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<IList<DishCategory>> GetAll() => await dbContext.DishCategories.ToListAsync();

    /// <inheritdoc />
    public async Task<DishCategory?> GetById(int id) => await dbContext.DishCategories.FirstOrDefaultAsync(c => c.Id == id);

    /// <inheritdoc />
    public async Task<DishCategory> Update(DishCategory entity)
    {
        var result = dbContext.DishCategories.Update(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}