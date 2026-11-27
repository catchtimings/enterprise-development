using Bogus;
using FoodDelivery.Contracts.Dtos.Orders;

namespace FoodDelivery.Generator.Domain;

/// <summary>
/// Генератор контрактов заказов на основе Bogus
/// </summary>
public class BogusOrderGenerator
{
    private readonly Faker<OrderCreateUpdateDto> _faker;

    /// <summary>
    /// Создаёт генератор с правилами для заказов
    /// </summary>
    public BogusOrderGenerator()
    {
        _faker = new Faker<OrderCreateUpdateDto>()
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
    }

    /// <summary>
    /// Генерирует один контракт заказа
    /// </summary>
    /// <returns>Контракт заказа</returns>
    public OrderCreateUpdateDto Generate() => _faker.Generate();

    /// <summary>
    /// Генерирует батч контрактов заказов
    /// </summary>
    /// <param name="count">Размер батча</param>
    /// <returns>Коллекция контрактов</returns>
    public IList<OrderCreateUpdateDto> GenerateBatch(int count) =>
        Enumerable.Range(0, count).Select(_ => Generate()).ToList();
}