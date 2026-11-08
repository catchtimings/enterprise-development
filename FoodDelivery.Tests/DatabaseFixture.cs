using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Tests;

/// <summary>
/// Контекст для общего использования датасидера между тестами
/// </summary>
public class DatabaseFixture
{
    public IReadOnlyList<Client> Clients { get; } = DataSeeder.Clients;

    public IReadOnlyList<Dish> Dishes { get; } = DataSeeder.Dishes;

    public IReadOnlyList<DishCategory> Categories { get; } = DataSeeder.Categories;

    public IReadOnlyList<Order> Orders { get; } = DataSeeder.Orders;

    public IReadOnlyList<Restaurant> Restaurants { get; } = DataSeeder.Restaurants;
}