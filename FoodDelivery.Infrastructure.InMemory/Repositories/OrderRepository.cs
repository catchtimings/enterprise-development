using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий заказов в памяти
/// </summary>
public class OrderRepository : IOrderRepository
{
    /// <inheritdoc />
    public Task<IList<Order>> GetAll() => Task.FromResult<IList<Order>>(DataSeeder.Orders);

    /// <inheritdoc />
    public Task<Order?> GetById(int id) => Task.FromResult(DataSeeder.Orders.FirstOrDefault(o => o.Id == id));

    /// <inheritdoc />
    public Task<Order> Add(Order entity)
    {
        entity.Id = DataSeeder.Orders.Any() ? DataSeeder.Orders.Max(o => o.Id) + 1 : 1;

        var nextItemId = DataSeeder.Orders.SelectMany(o => o.Items).Any()
            ? DataSeeder.Orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        foreach (var item in entity.Items)
        {
            item.Id = nextItemId++;
            item.OrderId = entity.Id;
        }

        DataSeeder.Orders.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Order> Update(Order entity)
    {
        var existing = DataSeeder.Orders.FirstOrDefault(o => o.Id == entity.Id);
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
        var existing = DataSeeder.Orders.FirstOrDefault(o => o.Id == id);
        if (existing == null) return Task.FromResult(false);

        DataSeeder.Orders.Remove(existing);
        return Task.FromResult(true);
    }
}