using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий ресторанов в базе данных
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class RestaurantEfCoreRepository(FoodDeliveryDbContext dbContext) : IRestaurantRepository
{
    /// <inheritdoc />
    public async Task<Restaurant> Add(Restaurant entity)
    {
        var result = await dbContext.Restaurants.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var entity = await dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        if (entity == null)
            return false;

        dbContext.Restaurants.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<IList<Restaurant>> GetAll() => await dbContext.Restaurants.ToListAsync();

    /// <inheritdoc />
    public async Task<Restaurant?> GetById(int id) => await dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

    /// <inheritdoc />
    public async Task<Restaurant> Update(Restaurant entity)
    {
        var result = dbContext.Restaurants.Update(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}