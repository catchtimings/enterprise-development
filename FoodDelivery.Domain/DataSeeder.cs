using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain;

/// <summary>
/// Генератор тестового датасета
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Список категорий
    /// </summary>
    public List<DishCategory> Categories { get; } = GetCategories();

    /// <summary>
    /// Список ресторанов
    /// </summary>
    public List<Restaurant> Restaurants { get; } = GetRestaurants();

    /// <summary>
    /// Список клиентов
    /// </summary>
    public List<Client> Clients { get; } = GetClients();

    /// <summary>
    /// Список блюд
    /// </summary>
    public List<Dish> Dishes { get; }

    /// <summary>
    /// Список заказов
    /// </summary>
    public List<Order> Orders { get; }

    public DataSeeder()
    {
        Dishes = GetDishes(Categories);
        Orders = GetOrders(Dishes, Clients, Restaurants);
    }

    /// <summary>
    /// Создаёт список категорий блюд
    /// </summary>
    /// <returns>Список категорий блюд</returns>
    private static List<DishCategory> GetCategories() =>
    [
        new DishCategory { Id = 1, Name = "Пицца" },
        new DishCategory { Id = 2, Name = "Суши" },
        new DishCategory { Id = 3, Name = "Бургеры" },
        new DishCategory { Id = 4, Name = "Супы" },
        new DishCategory { Id = 5, Name = "Салаты" },
        new DishCategory { Id = 6, Name = "Десерты" },
        new DishCategory { Id = 7, Name = "Напитки" },
        new DishCategory { Id = 8, Name = "Закуски" },
        new DishCategory { Id = 9, Name = "Паста" },
        new DishCategory { Id = 10, Name = "Гриль" }
    ];

    /// <summary>
    /// Создаёт список ресторанов
    /// </summary>
    /// <returns>Список ресторанов</returns>
    private static List<Restaurant> GetRestaurants() =>
    [
        new Restaurant { Id = 1, Name = "Ресторан №1", Address = "Ул. Ленина, д. 1", Rating = 4.2, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) },
        new Restaurant { Id = 2, Name = "Ресторан №2", Address = "Ул. Ленина, д. 2", Rating = 4.4, OpeningTime = new TimeOnly(9, 30), ClosingTime = new TimeOnly(23, 30) },
        new Restaurant { Id = 3, Name = "Ресторан №3", Address = "Ул. Ленина, д. 3", Rating = 4.6, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) },
        new Restaurant { Id = 4, Name = "Ресторан №4", Address = "Ул. Ленина, д. 4", Rating = 4.8, OpeningTime = new TimeOnly(9, 30), ClosingTime = new TimeOnly(23, 30) },
        new Restaurant { Id = 5, Name = "Ресторан №5", Address = "Ул. Ленина, д. 5", Rating = 4.0, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) },
        new Restaurant { Id = 6, Name = "Ресторан №6", Address = "Ул. Ленина, д. 6", Rating = 4.2, OpeningTime = new TimeOnly(9, 30), ClosingTime = new TimeOnly(23, 30) },
        new Restaurant { Id = 7, Name = "Ресторан №7", Address = "Ул. Ленина, д. 7", Rating = 4.4, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) },
        new Restaurant { Id = 8, Name = "Ресторан №8", Address = "Ул. Ленина, д. 8", Rating = 4.6, OpeningTime = new TimeOnly(9, 30), ClosingTime = new TimeOnly(23, 30) },
        new Restaurant { Id = 9, Name = "Ресторан №9", Address = "Ул. Ленина, д. 9", Rating = 4.8, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) },
        new Restaurant { Id = 10, Name = "Ресторан №10", Address = "Ул. Ленина, д. 10", Rating = 4.0, OpeningTime = new TimeOnly(9, 30), ClosingTime = new TimeOnly(23, 30) }
    ];

    /// <summary>
    /// Создаёт список клиентов
    /// </summary>
    /// <returns>Список клиентов</returns>
    private static List<Client> GetClients() =>
    [
        new Client { Id = 1, FullName = "Аннова Анна Ивановна", PhoneNumber = "+79990000000", DeliveryAddress = "Ул. Пушкина, 1, д. Колотушкина 11, кв. 101" },
        new Client { Id = 2, FullName = "Алексеев Алексей Алексеевич", PhoneNumber = "+79990000001", DeliveryAddress = "Ул. Пушкина, 2, д. Колотушкина 12, кв. 102" },
        new Client { Id = 3, FullName = "Васильев Василий Васильевич", PhoneNumber = "+79990000002", DeliveryAddress = "Ул. Пушкина, 3, д. Колотушкина 13, кв. 103" },
        new Client { Id = 4, FullName = "Дмитриев Дмитрий Дмитриевич", PhoneNumber = "+79990000003", DeliveryAddress = "Ул. Пушкина, 4, д. Колотушкина 14, кв. 104" },
        new Client { Id = 5, FullName = "Иванов Иван Иванович", PhoneNumber = "+79990000004", DeliveryAddress = "Ул. Пушкина, 5, д. Колотушкина 15, кв. 105" },
        new Client { Id = 6, FullName = "Ольгина Ольга Олеговна", PhoneNumber = "+79990000005", DeliveryAddress = "Ул. Пушкина, 6, д. Колотушкина 16, кв. 106" },
        new Client { Id = 7, FullName = "Павлов Павел Павлович", PhoneNumber = "+79990000006", DeliveryAddress = "Ул. Пушкина, 7, д. Колотушкина 17, кв. 107" },
        new Client { Id = 8, FullName = "Петров Петр Петрович", PhoneNumber = "+79990000007", DeliveryAddress = "Ул. Пушкина, 8, д. Колотушкина 18, кв. 108" },
        new Client { Id = 9, FullName = "Сергеев Сергей Сергеевич", PhoneNumber = "+79990000008", DeliveryAddress = "Ул. Пушкина, 9, д. Колотушкина 19, кв. 109" },
        new Client { Id = 10, FullName = "Сидоров Сидор Сидорович", PhoneNumber = "+79990000009", DeliveryAddress = "Ул. Пушкина, 10, д. Колотушкина 20, кв. 110" },
        new Client { Id = 11, FullName = "Смирнов Станислав Сергеевич", PhoneNumber = "+79990000010", DeliveryAddress = "Ул. Пушкина, 11, д. Колотушкина 21, кв. 111" }
    ];

    /// <summary>
    /// Создаёт список блюд
    /// </summary>
    /// <param name="categories">Список категорий блюд</param>
    /// <returns>Список блюд</returns>
    private static List<Dish> GetDishes(List<DishCategory> categories) =>
    [
        new Dish { Id = 1, Name = "Пепперони", Weight = 200, Price = 150, CategoryId = 1, Category = categories[0] },
        new Dish { Id = 2, Name = "Филадельфия", Weight = 250, Price = 250, CategoryId = 2, Category = categories[1] },
        new Dish { Id = 3, Name = "Чизбургер", Weight = 300, Price = 350, CategoryId = 3, Category = categories[2] },
        new Dish { Id = 4, Name = "Борщ", Weight = 350, Price = 450, CategoryId = 4, Category = categories[3] },
        new Dish { Id = 5, Name = "Цезарь", Weight = 400, Price = 550, CategoryId = 5, Category = categories[4] },
        new Dish { Id = 6, Name = "Чизкейк", Weight = 450, Price = 650, CategoryId = 6, Category = categories[5] },
        new Dish { Id = 7, Name = "Кола", Weight = 500, Price = 750, CategoryId = 7, Category = categories[6] },
        new Dish { Id = 8, Name = "Картофель фри", Weight = 550, Price = 850, CategoryId = 8, Category = categories[7] },
        new Dish { Id = 9, Name = "Карбонара", Weight = 600, Price = 950, CategoryId = 9, Category = categories[8] },
        new Dish { Id = 10, Name = "Стейк рибай", Weight = 650, Price = 1050, CategoryId = 10, Category = categories[9] }
    ];

    /// <summary>
    /// Создаёт список заказов
    /// </summary>
    /// <param name="dishes">Список блюд</param>
    /// <param name="clients">Список клиентов</param>
    /// <param name="restaurants">Список ресторанов</param>
    /// <returns>Список заказов</returns>
    private static List<Order> GetOrders(List<Dish> dishes, List<Client> clients, List<Restaurant> restaurants) =>
    [
        new Order { Id = 1, ClientId = 1, Client = clients[0], RestaurantId = 1, Restaurant = restaurants[0], CreatedAt = new DateTime(2026, 9, 10, 14, 0, 0), DeliveredAt = new DateTime(2026, 9, 10, 14, 15, 0), TotalAmount = 150, Items = [new OrderItem { Id = 1, OrderId = 1, DishId = 1, Dish = dishes[0], Quantity = 1, PriceAtOrder = 150 }] },
        new Order { Id = 2, ClientId = 2, Client = clients[1], RestaurantId = 2, Restaurant = restaurants[1], CreatedAt = new DateTime(2026, 9, 10, 16, 0, 0), DeliveredAt = new DateTime(2026, 9, 10, 16, 15, 0), TotalAmount = 500, Items = [new OrderItem { Id = 2, OrderId = 2, DishId = 2, Dish = dishes[1], Quantity = 2, PriceAtOrder = 250 }] },
        new Order { Id = 3, ClientId = 3, Client = clients[2], RestaurantId = 3, Restaurant = restaurants[2], CreatedAt = new DateTime(2026, 9, 10, 18, 0, 0), DeliveredAt = new DateTime(2026, 9, 10, 18, 35, 0), TotalAmount = 1050, Items = [new OrderItem { Id = 3, OrderId = 3, DishId = 3, Dish = dishes[2], Quantity = 3, PriceAtOrder = 350 }] },
        new Order { Id = 4, ClientId = 4, Client = clients[3], RestaurantId = 4, Restaurant = restaurants[3], CreatedAt = new DateTime(2026, 9, 10, 20, 0, 0), DeliveredAt = new DateTime(2026, 9, 10, 20, 40, 0), TotalAmount = 1800, Items = [new OrderItem { Id = 4, OrderId = 4, DishId = 4, Dish = dishes[3], Quantity = 4, PriceAtOrder = 450 }] },
        new Order { Id = 5, ClientId = 5, Client = clients[4], RestaurantId = 5, Restaurant = restaurants[4], CreatedAt = new DateTime(2026, 9, 11, 14, 0, 0), DeliveredAt = new DateTime(2026, 9, 11, 14, 45, 0), TotalAmount = 2750, Items = [new OrderItem { Id = 5, OrderId = 5, DishId = 5, Dish = dishes[4], Quantity = 5, PriceAtOrder = 550 }] },
        new Order { Id = 6, ClientId = 6, Client = clients[5], RestaurantId = 6, Restaurant = restaurants[5], CreatedAt = new DateTime(2026, 9, 11, 16, 0, 0), DeliveredAt = new DateTime(2026, 9, 11, 16, 50, 0), TotalAmount = 3900, Items = [new OrderItem { Id = 6, OrderId = 6, DishId = 6, Dish = dishes[5], Quantity = 6, PriceAtOrder = 650 }] },
        new Order { Id = 7, ClientId = 7, Client = clients[6], RestaurantId = 7, Restaurant = restaurants[6], CreatedAt = new DateTime(2026, 9, 11, 18, 0, 0), DeliveredAt = new DateTime(2026, 9, 11, 18, 55, 0), TotalAmount = 5250, Items = [new OrderItem { Id = 7, OrderId = 7, DishId = 7, Dish = dishes[6], Quantity = 7, PriceAtOrder = 750 }] },
        new Order { Id = 8, ClientId = 8, Client = clients[7], RestaurantId = 8, Restaurant = restaurants[7], CreatedAt = new DateTime(2026, 9, 11, 20, 0, 0), DeliveredAt = new DateTime(2026, 9, 11, 21, 0, 0), TotalAmount = 6800, Items = [new OrderItem { Id = 8, OrderId = 8, DishId = 8, Dish = dishes[7], Quantity = 8, PriceAtOrder = 850 }] },
        new Order { Id = 9, ClientId = 9, Client = clients[8], RestaurantId = 9, Restaurant = restaurants[8], CreatedAt = new DateTime(2026, 9, 12, 14, 0, 0), DeliveredAt = new DateTime(2026, 9, 12, 15, 5, 0), TotalAmount = 8550, Items = [new OrderItem { Id = 9, OrderId = 9, DishId = 9, Dish = dishes[8], Quantity = 9, PriceAtOrder = 950 }] },
        new Order { Id = 10, ClientId = 10, Client = clients[9], RestaurantId = 10, Restaurant = restaurants[9], CreatedAt = new DateTime(2026, 9, 12, 16, 0, 0), DeliveredAt = new DateTime(2026, 9, 12, 17, 10, 0), TotalAmount = 10500, Items = [new OrderItem { Id = 10, OrderId = 10, DishId = 10, Dish = dishes[9], Quantity = 10, PriceAtOrder = 1050 }] },
        new Order { Id = 11, ClientId = 11, Client = clients[10], RestaurantId = 10, Restaurant = restaurants[9], CreatedAt = new DateTime(2026, 9, 12, 18, 0, 0), DeliveredAt = new DateTime(2026, 9, 12, 19, 10, 0), TotalAmount = 10500, Items = [ new OrderItem { Id = 11, OrderId = 11, DishId = 10, Dish = dishes[9], Quantity = 10, PriceAtOrder = 1050 } ] }
    ];
}