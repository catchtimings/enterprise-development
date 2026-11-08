using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий заказов в базе данных
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class OrderEfCoreRepository(FoodDeliveryDbContext dbContext) : IOrderRepository
{
    /// <inheritdoc />
    public async Task<Order> Add(Order entity)
    {
        var result = await dbContext.Orders.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var entity = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (entity == null)
            return false;

        dbContext.Orders.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<IList<Order>> GetAll() =>
        await dbContext.Orders
            .Include(o => o.Client)
            .Include(o => o.Restaurant)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
            .AsSplitQuery()
            .ToListAsync();

    /// <inheritdoc />
    public async Task<Order?> GetById(int id) =>
        await dbContext.Orders
            .Include(o => o.Client)
            .Include(o => o.Restaurant)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == id);

    /// <inheritdoc />
    public async Task<Order> Update(Order entity)
    {
        var existing = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == entity.Id);

        if (existing == null)
            throw new KeyNotFoundException($"Заказ с Id {entity.Id} не найден");

        existing.ClientId = entity.ClientId;
        existing.RestaurantId = entity.RestaurantId;
        existing.CreatedAt = entity.CreatedAt;
        existing.DeliveredAt = entity.DeliveredAt;
        existing.TotalAmount = entity.TotalAmount;

        dbContext.OrderItems.RemoveRange(existing.Items);
        existing.Items = entity.Items;

        await dbContext.SaveChangesAsync();
        return existing;
    }
}