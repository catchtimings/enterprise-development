using Bogus;
using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Generator.Domain;

/// <summary>
/// Генератор контрактов заказов
/// </summary>
public static class OrderGenerator
{
    private static readonly Faker<OrderCreateUpdateDto> _faker = new Faker<OrderCreateUpdateDto>()
        .CustomInstantiator(f =>
        {
            var items = Enumerable.Range(0, f.Random.Int(1, 5))
                .Select(_ => new OrderItemCreateUpdateDto(
                    DishId: f.Random.Int(1, 13),
                    Quantity: f.Random.Int(1, 5)))
                .ToList();

            DateTime? deliveredAt = f.Random.Bool(0.7f)
                ? DateTime.UtcNow.AddMinutes(f.Random.Int(10, 90))
                : null;

            return new OrderCreateUpdateDto(
                ClientId: f.Random.Int(1, 25),
                RestaurantId: f.Random.Int(1, 15),
                DeliveredAt: deliveredAt,
                Items: items);
        });

    /// <summary>
    /// Генерирует заданное количество контрактов заказов
    /// </summary>
    /// <param name="count">Размер батча</param>
    /// <returns>Коллекция контрактов</returns>
    public static IList<OrderCreateUpdateDto> GenerateBatch(int count) => _faker.Generate(count);
}