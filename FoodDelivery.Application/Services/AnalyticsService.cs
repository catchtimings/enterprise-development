using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис аналитики
/// </summary>
public class AnalyticsService(
    IRepository<Order> orderRepository,
    IRepository<Restaurant> restaurantRepository)
{
    /// <summary>
    /// Возвращает топ-5 ресторанов по количеству заказов
    /// </summary>
    public async Task<IEnumerable<RestaurantWithOrderCountDto>> GetTop5Restaurants()
    {
        var orders = await orderRepository.GetAll();
        var restaurants = await restaurantRepository.GetAll();

        return orders
            .GroupBy(o => o.RestaurantId)
            .Select(g => new
            {
                Restaurant = restaurants.FirstOrDefault(r => r.Id == g.Key),
                OrderCount = g.Count()
            })
            .Where(x => x.Restaurant != null)
            .OrderByDescending(x => x.OrderCount)
            .ThenBy(x => x.Restaurant!.Id)
            .Take(5)
            .Select(x => new RestaurantWithOrderCountDto
            {
                Restaurant = RestaurantMapper.ToDto(x.Restaurant!),
                OrderCount = x.OrderCount
            })
            .ToList();
    }

    /// <summary>
    /// Возвращает заказы с минимальным временем доставки
    /// </summary>
    public async Task<MinDeliveryTimeOrdersDto> GetMinDeliveryTimeOrders()
    {
        var orders = await orderRepository.GetAll();
        if (!orders.Any())
        {
            return new MinDeliveryTimeOrdersDto { MinDeliveryTime = TimeSpan.Zero, Orders = [] };
        }

        var minDuration = orders.Min(o => o.DeliveryDuration);
        var result = orders
            .Where(o => o.DeliveryDuration == minDuration)
            .Select(OrderMapper.ToDto)
            .ToList();

        return new MinDeliveryTimeOrdersDto
        {
            MinDeliveryTime = minDuration,
            Orders = result
        };
    }

    /// <summary>
    /// Возвращает клиентов указанного ресторана, отсортированных по ФИО
    /// </summary>
    public async Task<IEnumerable<ClientDto>> GetClientsByRestaurant(int restaurantId)
    {
        var orders = await orderRepository.GetAll();

        var clients = orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        return clients.Select(ClientMapper.ToDto);
    }

    /// <summary>
    /// Возвращает сводную статистику по категориям блюд за указанный период
    /// </summary>
    public async Task<IEnumerable<CategorySummaryDto>> GetCategorySummary(DateTime startDate, DateTime endDate)
    {
        var orders = await orderRepository.GetAll();

        return orders
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
    }

    /// <summary>
    /// Возвращает клиентов с наибольшей суммой трат за всё время
    /// </summary>
    public async Task<IEnumerable<TopSpenderDto>> GetTopSpenders()
    {
        var orders = await orderRepository.GetAll();

        var clientSpending = orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!)
            .Select(g => new { Client = g.Key, TotalSpent = g.Sum(o => o.TotalAmount) })
            .ToList();

        if (!clientSpending.Any()) return [];

        var maxTotalSpent = clientSpending.Max(x => x.TotalSpent);

        return clientSpending
            .Where(x => x.TotalSpent == maxTotalSpent)
            .OrderBy(x => x.Client.Id)
            .Select(x => new TopSpenderDto
            {
                Client = ClientMapper.ToDto(x.Client),
                TotalSpent = x.TotalSpent
            })
            .ToList();
    }
}