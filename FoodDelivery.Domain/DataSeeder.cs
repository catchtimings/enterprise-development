using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain;

/// <summary>
/// Генератор тестового датасета
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Количество экземпляров каждой сущности
    /// </summary>
    private const int SeedSize = 10;

    public List<DishCategory> Categories { get; } = [];
    public List<Restaurant> Restaurants { get; } = [];
    public List<Client> Clients { get; } = [];
    public List<Dish> Dishes { get; } = [];
    public List<Order> Orders { get; } = [];

    public DataSeeder()
    {
        SeedData();
    }

    private void SeedData()
    {
        SeedCategories();
        SeedRestaurants();
        SeedClients();
        SeedDishes();
        SeedOrders();
    }

    private void SeedCategories()
    {
        var names = new[]
        {
            "Пицца", "Суши", "Бургеры", "Супы", "Салаты",
            "Десерты", "Напитки", "Закуски", "Паста", "Гриль"
        };

        for (var i = 1; i <= SeedSize; i++)
        {
            Categories.Add(new DishCategory
            {
                Id = i,
                Name = names[i - 1]
            });
        }
    }

    private void SeedRestaurants()
    {
        for (var i = 1; i <= SeedSize; i++)
        {
            Restaurants.Add(new Restaurant
            {
                Id = i,
                Name = $"Ресторан №{i}",
                Address = $"Ул. Ленина, д. {i}",
                Rating = 4.0 + (i % 5) * 0.2,
                OpeningTime = new TimeOnly(9, 0, 0),
                ClosingTime = new TimeOnly(23, 0, 0)
            });
        }
    }

    private void SeedClients()
    {
        var names = new[]
        {
            "Аннова Анна Ивановна", "Алексеев Алексей Алексеевич", "Васильев Василий Васильевич",
            "Дмитриев Дмитрий Дмитриевич", "Иванов Иван Иванович", "Ольгина Ольга Олеговна",
            "Павлов Павел Павлович", "Петров Петр Петрович", "Сергеев Сергей Сергеевич", "Сидоров Сидор Сидорович"
        };

        for (var i = 1; i <= SeedSize; i++)
        {
            Clients.Add(new Client
            {
                Id = i,
                FullName = names[i - 1],
                PhoneNumber = $"+7999000000{i - 1}",
                DeliveryAddress = $"Ул. Пушкина, {i}, д. Колотушкина {i + 10}, кв. {i + 100}"
            });
        }
    }

    private void SeedDishes()
    {
        for (var i = 1; i <= SeedSize; i++)
        {
            var category = Categories[i - 1];
            Dishes.Add(new Dish
            {
                Id = i,
                Name = $"Блюдо {i}",
                WeightInGrams = 150 + i * 50,
                Price = 150 + (i - 1) * 100,
                CategoryId = category.Id,
                Category = category
            });
        }
    }

    private void SeedOrders()
    {
        var baseTime = DateTimeOffset.UtcNow.AddDays(-5);

        for (var i = 1; i <= SeedSize; i++)
        {
            var dish = Dishes[i - 1];
            var client = Clients[i - 1];
            var restaurant = Restaurants[i - 1];

            var deliveryDelayMinutes = i <= 2 ? 15 : 20 + i * 5;

            var order = new Order
            {
                Id = i,
                ClientId = client.Id,
                Client = client,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                CreatedAt = baseTime.AddHours(i * 2),
                DeliveredAt = baseTime.AddHours(i * 2).AddMinutes(deliveryDelayMinutes),
                TotalAmount = i * dish.Price
            };

            var orderItem = new OrderItem
            {
                Id = i,
                OrderId = order.Id,
                DishId = dish.Id,
                Dish = dish,
                Quantity = i,
                PriceAtOrder = dish.Price
            };

            order.Items.Add(orderItem);
            Orders.Add(order);
        }
    }
}