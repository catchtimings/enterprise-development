using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain;

/// <summary>
/// Генератор тестового датасета
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Список категорий блюд
    /// </summary>
    public List<DishCategory> Categories { get; } = [];

    /// <summary>
    /// Список ресторанов
    /// </summary>
    public List<Restaurant> Restaurants { get; } = [];

    /// <summary>
    /// Список клиентов
    /// </summary>
    public List<Client> Clients { get; } = [];

    /// <summary>
    /// Список блюд
    /// </summary>
    public List<Dish> Dishes { get; } = [];

    /// <summary>
    /// Список заказов
    /// </summary>
    public List<Order> Orders { get; } = [];

    public DataSeeder()
    {
        SeedData();
    }

    private void SeedData()
    {
        var categoryNames = new[] { "Пицца", "Суши", "Бургеры", "Супы", "Салаты", "Десерты", "Напитки", "Закуски", "Паста", "Гриль" };
        for (var i = 0; i < 10; i++)
        {
            Categories.Add(new DishCategory
            {
                Id = Guid.NewGuid(),
                Name = categoryNames[i]
            });
        }

        for (var i = 1; i <= 10; i++)
        {
            Restaurants.Add(new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = $"Ресторан №{i}",
                Address = $"Ул. Ленина, д. {i}",
                Rating = 4.0 + (i % 5) * 0.2,
                OpeningHours = new TimeOnly(09, 00, 00),
                ClosingHours = new TimeOnly(23, 00, 00)
            });
        }

        var names = new[]
        {
            "Аннова Анна Ивановна", "Алексеев Алексей Алексеевич", "Васильев Василий Васильевич",
            "Дмитриев Дмитрий Дмитриевич", "Иванов Иван Иванович", "Ольгина Ольга Олеговна",
            "Павлов Павел Павлович", "Петров Петр Петрович", "Сергеев Сергей Сергеевич", "Сидоров Сидор Сидорович"
        };

        for (var i = 0; i < 10; i++)
        {
            Clients.Add(new Client
            {
                Id = Guid.NewGuid(),
                FullName = names[i],
                PhoneNumber = $"+7999000000{i}",
                DeliveryAddress = $"Ул. Пушкина, {i + 1}, д. Колотушкина {i + 10}, кв. {i + 100}"
            });
        }

        for (var i = 0; i < 10; i++)
        {
            Dishes.Add(new Dish
            {
                Id = Guid.NewGuid(),
                Name = $"Блюдо {i + 1}",
                WeightInGrams = 200 + i * 50,
                Price = 150 + i * 100,
                CategoryId = Categories[i].Id,
                Category = Categories[i]
            });
        }

        var baseTime = DateTimeOffset.UtcNow.AddDays(-5);
        for (var i = 0; i < 10; i++)
        {
            var dish = Dishes[i];
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                DishId = dish.Id,
                Dish = dish,
                Quantity = i + 1,
                PriceAtOrder = dish.Price
            };

            var deliveryDelayMinutes = (i < 2) ? 15 : 20 + i * 5;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ClientId = Clients[i].Id,
                Client = Clients[i],
                RestaurantId = Restaurants[i % Restaurants.Count].Id,
                Restaurant = Restaurants[i % Restaurants.Count],
                CreatedAt = baseTime.AddHours(i * 2),
                DeliveredAt = baseTime.AddHours(i * 2).AddMinutes(deliveryDelayMinutes),
                TotalAmount = orderItem.Quantity * orderItem.PriceAtOrder,
                Items = [orderItem]
            };

            orderItem.OrderId = order.Id;
            Orders.Add(order);
        }
    }
}