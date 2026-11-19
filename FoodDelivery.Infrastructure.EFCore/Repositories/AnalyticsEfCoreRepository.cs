using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий аналитики на основе Entity Framework Core
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class AnalyticsEfCoreRepository(FoodDeliveryDbContext dbContext) : IAnalyticsRepository
{
    /// <inheritdoc />
    public async Task<IList<(int RestaurantId, int OrderCount)>> GetRestaurantOrderCounts()
    {
        var result = await dbContext.Orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => new { RestaurantId = g.Key, OrderCount = g.Count() })
            .ToListAsync();

        return result.Select(x => (x.RestaurantId, x.OrderCount)).ToList();
    }

    /// <inheritdoc />
    public async Task<IList<Order>> GetOrdersWithMinDelivery()
    {
        var hasDeliveredOrders = await dbContext.Orders
            .AnyAsync(o => o.DeliveredAt != null);

        if (!hasDeliveredOrders)
            return [];

        var minDurationMs = await dbContext.Orders
            .Where(o => o.DeliveredAt != null)
            .MinAsync(o => EF.Functions.DateDiffMillisecond(o.CreatedAt, o.DeliveredAt!.Value));

        var result = await dbContext.Orders
            .Include(o => o.Client)
            .Include(o => o.Restaurant)
            .Include(o => o.Items).ThenInclude(i => i.Dish)
            .Where(o => o.DeliveredAt != null && EF.Functions.DateDiffMillisecond(o.CreatedAt, o.DeliveredAt.Value) == minDurationMs)
            .AsSplitQuery()
            .ToListAsync();

        return result;
    }

    /// <inheritdoc />
    public async Task<IList<Client>> GetClientsByRestaurant(int restaurantId)
    {
        var result = await dbContext.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .Distinct()
            .ToListAsync();

        return result;
    }

    /// <inheritdoc />
    public async Task<IList<(string CategoryName, int OrderCount, decimal AverageSum, decimal TotalSum)>> GetCategorySummary(DateTime startDate, DateTime endDate)
    {
        var result = await dbContext.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(i => i.Dish != null && i.Dish.Category != null)
            .GroupBy(i => i.Dish!.Category!.Name)
            .Select(g => new
            {
                CategoryName = g.Key,
                OrderCount = g.Select(i => i.OrderId).Distinct().Count(),
                AverageSum = g.Average(i => i.Quantity * i.PriceAtOrder),
                TotalSum = g.Sum(i => i.Quantity * i.PriceAtOrder)
            })
            .ToListAsync();

        return result.Select(x => (x.CategoryName, x.OrderCount, x.AverageSum, x.TotalSum)).ToList();
    }

    /// <inheritdoc />
    public async Task<IList<(Client Client, decimal TotalSpent)>> GetClientSpendingTotals()
    {
        var spending = dbContext.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            });

        var result = await spending
            .Join(
                dbContext.Clients,
                s => s.ClientId,
                c => c.Id,
                (s, c) => new
                {
                    Client = c,
                    s.TotalSpent
                })
            .ToListAsync();

        return result.Select(x => (x.Client, x.TotalSpent)).ToList();
    }
}