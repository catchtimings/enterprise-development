using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий аналитики в памяти
/// </summary>
public class AnalyticsInMemoryRepository : IAnalyticsRepository
{
    /// <inheritdoc />
    public Task<IList<(int RestaurantId, int OrderCount)>> GetRestaurantOrderCounts()
    {
        var result = DataSeeder.Orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => (g.Key, g.Count()))
            .ToList();

        return Task.FromResult<IList<(int, int)>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Order>> GetOrdersWithMinDelivery()
    {
        var delivered = DataSeeder.Orders.Where(o => o.DeliveryDuration.HasValue).ToList();
        if (!delivered.Any()) return Task.FromResult<IList<Order>>(new List<Order>());

        var minMs = delivered.Min(o => o.DeliveryDuration!.Value.TotalMilliseconds);
        var result = delivered.Where(o => o.DeliveryDuration!.Value.TotalMilliseconds == minMs).ToList();

        return Task.FromResult<IList<Order>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Client>> GetClientsByRestaurant(int restaurantId)
    {
        var result = DataSeeder.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .ToList();

        return Task.FromResult<IList<Client>>(result);
    }

    /// <inheritdoc />
    public Task<IList<(string CategoryName, int OrderCount, decimal AverageSum, decimal TotalSum)>> GetCategorySummary(DateTime startDate, DateTime endDate)
    {
        var result = DataSeeder.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(i => i.Dish?.Category != null)
            .GroupBy(i => i.Dish!.Category!.Name)
            .Select(g => (
                CategoryName: g.Key,
                OrderCount: g.Select(i => i.OrderId).Distinct().Count(),
                AverageSum: g.Average(i => i.Quantity * i.PriceAtOrder),
                TotalSum: g.Sum(i => i.Quantity * i.PriceAtOrder)
            ))
            .ToList();

        return Task.FromResult<IList<(string, int, decimal, decimal)>>(result);
    }

    /// <inheritdoc />
    public Task<IList<(Client Client, decimal TotalSpent)>> GetClientSpendingTotals()
    {
        var result = DataSeeder.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!.Id)
            .Select(g => (
                Client: g.First().Client!,
                TotalSpent: g.Sum(o => o.TotalAmount)
            ))
            .ToList();

        return Task.FromResult<IList<(Client, decimal)>>(result);
    }
}