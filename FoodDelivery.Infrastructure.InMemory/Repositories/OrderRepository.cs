using FoodDelivery.Application.Interfaces;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий заказов в памяти
/// </summary>
public class OrderRepository(DataSeeder seeder) : IRepository<Order>
{
    /// <inheritdoc />
    public Task<IEnumerable<Order>> GetAll() => Task.FromResult<IEnumerable<Order>>(seeder.Orders);

    /// <inheritdoc />
    public Task<Order?> GetById(int id) => Task.FromResult(seeder.Orders.FirstOrDefault(o => o.Id == id));

    /// <inheritdoc />
    public Task Add(Order entity)
    {
        entity.Id = seeder.Orders.Any() ? seeder.Orders.Max(o => o.Id) + 1 : 1;

        var nextItemId = seeder.Orders.SelectMany(o => o.Items).Any()
            ? seeder.Orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        foreach (var item in entity.Items)
        {
            item.Id = nextItemId++;
            item.OrderId = entity.Id;
        }

        seeder.Orders.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task Update(Order entity)
    {
        var existing = seeder.Orders.FirstOrDefault(o => o.Id == entity.Id);
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
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task Delete(int id)
    {
        var existing = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (existing != null) seeder.Orders.Remove(existing);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> IsExists(int id) => Task.FromResult(seeder.Orders.Any(o => o.Id == id));
}