using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий заказов в памяти
/// </summary>
public class OrderInMemoryRepository : IOrderRepository
{
    private readonly List<Order> _orders = [.. DataSeeder.Orders];

    /// <inheritdoc />
    public Task<IList<Order>> GetAll() => Task.FromResult<IList<Order>>(_orders);

    /// <inheritdoc />
    public Task<Order?> GetById(int id) => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    /// <inheritdoc />
    public Task<Order> Add(Order entity)
    {
        entity.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;

        var nextItemId = _orders.SelectMany(o => o.Items).Any()
            ? _orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        foreach (var item in entity.Items)
        {
            item.Id = nextItemId++;
            item.OrderId = entity.Id;
        }

        _orders.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Order> Update(Order entity)
    {
        var existing = _orders.FirstOrDefault(o => o.Id == entity.Id);
        if (existing != null)
        {
            existing.ClientId = entity.ClientId;
            existing.Client = entity.Client;
            existing.RestaurantId = entity.RestaurantId;
            existing.Restaurant = entity.Restaurant;
            existing.CreatedAt = entity.CreatedAt;
            existing.DeliveredAt = entity.DeliveredAt;
            existing.Items = entity.Items;
        }
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = _orders.FirstOrDefault(o => o.Id == id);
        if (existing == null) return Task.FromResult(false);

        _orders.Remove(existing);
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<IList<(int RestaurantId, int OrderCount)>> GetRestaurantOrderCounts()
    {
        var result = _orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => (g.Key, g.Count()))
            .ToList();

        return Task.FromResult<IList<(int, int)>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Order>> GetOrdersWithMinDelivery()
    {
        var delivered = _orders.Where(o => o.DeliveryDuration.HasValue).ToList();
        if (!delivered.Any()) return Task.FromResult<IList<Order>>(new List<Order>());

        var minMs = delivered.Min(o => o.DeliveryDuration!.Value.TotalMilliseconds);
        var result = delivered.Where(o => o.DeliveryDuration!.Value.TotalMilliseconds == minMs).ToList();

        return Task.FromResult<IList<Order>>(result);
    }

    /// <inheritdoc />
    public Task<IList<Client>> GetClientsByRestaurant(int restaurantId)
    {
        var result = _orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .ToList();

        return Task.FromResult<IList<Client>>(result);
    }

    /// <inheritdoc />
    public Task<IList<(string CategoryName, int OrderCount, decimal AverageSum, decimal TotalSum)>> GetCategorySummary(DateTime startDate, DateTime endDate)
    {
        var result = _orders
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
        var result = _orders
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