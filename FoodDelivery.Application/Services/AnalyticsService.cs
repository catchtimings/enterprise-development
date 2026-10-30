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
    /// Проверяет получение пяти ресторанов с наибольшим количеством заказов
    /// </summary>
    public async Task<IList<RestaurantWithOrderCountDto>> GetTop5RestaurantsByOrderCount()
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
            .Select(x => new RestaurantWithOrderCountDto(
                RestaurantMapper.Map(x.Restaurant!),
                x.OrderCount
            ))
            .ToList();
    }

    /// <summary>
    /// Проверяет получение заказов с минимальным временем доставки
    /// </summary>
    public async Task<MinDeliveryTimeOrdersDto> GetOrdersWithMinDelivery()
    {
        var orders = await orderRepository.GetAll();

        var deliveredOrders = orders.Where(o => o.DeliveryDuration.HasValue).ToList();

        if (!deliveredOrders.Any())
        {
            return new MinDeliveryTimeOrdersDto(TimeSpan.Zero, []);
        }

        var minDuration = deliveredOrders.Min(o => o.DeliveryDuration!.Value);

        var result = deliveredOrders
            .Where(o => o.DeliveryDuration == minDuration)
            .Select(OrderMapper.Map)
            .ToList();

        return new MinDeliveryTimeOrdersDto(minDuration, result);
    }

    /// <summary>
    /// Проверяет получение клиентов выбранного ресторана, отсортированных по ФИО
    /// </summary>
    public async Task<IList<ClientDto>> GetClientsByRestaurant(int restaurantId)
    {
        var orders = await orderRepository.GetAll();

        var clients = orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        return ClientMapper.Map(clients);
    }

    /// <summary>
    /// Проверяет получение агрегированной статистики по категориям за заданный период
    /// </summary>
    public async Task<IList<CategorySummaryDto>> GetCategoryOrderSummary(DateTime startDate, DateTime endDate)
    {
        var orders = await orderRepository.GetAll();

        return orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .GroupBy(item => item.Dish!.Category!)
            .Select(g => new CategorySummaryDto(
                g.Key.Name,
                g.Select(item => item.OrderId).Distinct().Count(),
                g.Average(item => item.Quantity * item.PriceAtOrder),
                g.Sum(item => item.Quantity * item.PriceAtOrder)
            ))
            .OrderBy(s => s.CategoryName)
            .ToList();
    }

    /// <summary>
    /// Проверяет получение всех клиентов с максимальной общей суммой заказов
    /// </summary>
    public async Task<IList<TopSpenderDto>> GetTopSpenderClients()
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
            .Select(x => new TopSpenderDto(
                ClientMapper.Map(x.Client),
                x.TotalSpent
            ))
            .ToList();
    }
}