using FoodDelivery.Domain;

namespace FoodDelivery.Tests;

/// <summary>
/// Контекст для общего использования датасидера между тестами
/// </summary>
public class DatabaseFixture
{
    public DataSeeder Seeder { get; } = new();
}