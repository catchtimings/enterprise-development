using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Contracts.Dtos.Restaurants;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис бизнес-логики и обработки аналитических запросов
/// </summary>
/// <param name="seeder">Генератор и хранитель тестового датасета</param>
public class AnalyticsService(DataSeeder seeder)
{
    /// <summary>
    /// Выводит список из пяти ресторанов с наибольшим количеством заказов
    /// </summary>
    public Task<List<RestaurantWithOrderCountDto>> GetTop5RestaurantsByOrderCountAsync()
    {
        var result = seeder.Orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => new
            {
                Restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == g.Key),
                OrderCount = g.Count()
            })
            .Where(x => x.Restaurant is not null)
            .OrderByDescending(x => x.OrderCount)
            .ThenBy(x => x.Restaurant!.Id)
            .Take(5)
            .Select(x => new RestaurantWithOrderCountDto
            {
                Restaurant = MapRestaurantToDto(x.Restaurant!),
                OrderCount = x.OrderCount
            })
            .ToList();

        return Task.FromResult(result);
    }

    /// <summary>
    /// Выводит заказы с минимальным временем доставки и само это время
    /// </summary>
    public Task<MinDeliveryTimeOrdersDto> GetOrdersWithMinDeliveryTimeAsync()
    {
        if (!seeder.Orders.Any())
        {
            return Task.FromResult(new MinDeliveryTimeOrdersDto
            {
                MinDeliveryTime = TimeSpan.Zero,
                Orders = []
            });
        }

        var minDuration = seeder.Orders.Min(o => o.DeliveryDuration);
        var orders = seeder.Orders
            .Where(o => o.DeliveryDuration == minDuration)
            .Select(MapOrderToDto)
            .ToList();

        return Task.FromResult(new MinDeliveryTimeOrdersDto
        {
            MinDeliveryTime = minDuration,
            Orders = orders
        });
    }

    /// <summary>
    /// Выводит клиентов указанного ресторана с сортировкой по ФИО
    /// </summary>
    public Task<List<ClientDto>> GetClientsByRestaurantAsync(int restaurantId)
    {
        var clients = seeder.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .Select(MapClientToDto)
            .ToList();

        return Task.FromResult(clients);
    }

    /// <summary>
    /// Выводит сводную статистику по категориям блюд за период
    /// </summary>
    public Task<List<CategorySummaryDto>> GetCategoryOrderSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var summary = seeder.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .GroupBy(item => item.Dish!.Category!)
            .Select(g => new CategorySummaryDto
            {
                CategoryName = g.Key.Name,
                OrderCount = g.Select(item => item.OrderId).Distinct().Count(),
                AverageSum = g.Average(item => item.Quantity * item.PriceAtOrder),
                TotalSum = g.Sum(item => item.Quantity * item.PriceAtOrder)
            })
            .OrderBy(s => s.CategoryName)
            .ToList();

        return Task.FromResult(summary);
    }

    /// <summary>
    /// Выводит клиентов с максимальной суммой трат
    /// </summary>
    public Task<List<TopSpenderDto>> GetTopSpenderClientsAsync()
    {
        var clientSpending = seeder.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!)
            .Select(g => new
            {
                Client = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .ToList();

        if (!clientSpending.Any())
        {
            return Task.FromResult(new List<TopSpenderDto>());
        }

        var maxTotalSpent = clientSpending.Max(x => x.TotalSpent);

        var result = clientSpending
            .Where(x => x.TotalSpent == maxTotalSpent)
            .OrderBy(x => x.Client.Id)
            .Select(x => new TopSpenderDto
            {
                Client = MapClientToDto(x.Client),
                TotalSpent = x.TotalSpent
            })
            .ToList();

        return Task.FromResult(result);
    }

    private static ClientDto MapClientToDto(Client client) => new()
    {
        Id = client.Id,
        FullName = client.FullName,
        PhoneNumber = client.PhoneNumber,
        DeliveryAddress = client.DeliveryAddress
    };

    private static RestaurantDto MapRestaurantToDto(Restaurant restaurant) => new()
    {
        Id = restaurant.Id,
        Name = restaurant.Name,
        Address = restaurant.Address,
        Rating = restaurant.Rating,
        OpeningTime = restaurant.OpeningTime,
        ClosingTime = restaurant.ClosingTime
    };

    private static OrderDto MapOrderToDto(Order order) => new()
    {
        Id = order.Id,
        ClientId = order.ClientId,
        ClientName = order.Client?.FullName,
        RestaurantId = order.RestaurantId,
        RestaurantName = order.Restaurant?.Name,
        CreatedAt = order.CreatedAt,
        DeliveredAt = order.DeliveredAt,
        TotalAmount = order.TotalAmount,
        DeliveryDuration = order.DeliveryDuration,
        Items = order.Items.Select(i => new OrderItemDto
        {
            Id = i.Id,
            OrderId = i.OrderId,
            DishId = i.DishId,
            DishName = i.Dish?.Name,
            Quantity = i.Quantity,
            PriceAtOrder = i.PriceAtOrder
        }).ToList()
    };
}