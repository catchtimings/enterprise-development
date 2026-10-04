using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис бизнес-логики и обработки аналитических запросов
/// </summary>
/// <param name="seeder">Генератор и хранитель тестового датасета</param>
/// <param name="mapper">AutoMapper для преобразования сущностей в DTO</param>
public class AnalyticsService(DataSeeder seeder, IMapper mapper)
{
    /// <summary>
    /// Выводит список из пяти ресторанов с наибольшим количеством заказов
    /// </summary>
    /// <returns>Список DTO ресторанов, входящих в топ-5</returns>
    public Task<List<RestaurantDto>> GetTop5RestaurantsByOrderCountAsync()
    {
        var topIds = seeder.Orders
            .GroupBy(o => o.RestaurantId)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        var result = seeder.Restaurants
            .Where(r => topIds.Contains(r.Id))
            .ToList();

        return Task.FromResult(mapper.Map<List<RestaurantDto>>(result));
    }

    /// <summary>
    /// Выводит список заказов с минимальным временем доставки
    /// </summary>
    /// <returns>Список DTO быстрых заказов</returns>
    public Task<List<OrderDto>> GetOrdersWithMinDeliveryTimeAsync()
    {
        if (!seeder.Orders.Any()) return Task.FromResult(new List<OrderDto>());

        var minDuration = seeder.Orders.Min(o => o.DeliveryDuration);
        var orders = seeder.Orders
            .Where(o => o.DeliveryDuration == minDuration)
            .ToList();

        return Task.FromResult(mapper.Map<List<OrderDto>>(orders));
    }

    /// <summary>
    /// Выводит сведения обо всех клиентах, оформлявших заказы в указанном ресторане, с сортировкой по ФИО
    /// </summary>
    /// <param name="restaurantId">Идентификатор выбранного ресторана</param>
    /// <returns>Список DTO клиентов ресторана, отсортированный по алфавиту</returns>
    public Task<List<ClientDto>> GetClientsByRestaurantAsync(int restaurantId)
    {
        var clients = seeder.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        return Task.FromResult(mapper.Map<List<ClientDto>>(clients));
    }

    /// <summary>
    /// Выводит сводную статистику по категориям блюд за заданный период
    /// </summary>
    /// <param name="startDate">Начальная дата и время периода</param>
    /// <param name="endDate">Конечная дата и время периода</param>
    /// <returns>Список DTO с агрегированными показателями по каждой категории</returns>
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
    /// Выводит информацию о всех клиентах с максимальной суммой заказа
    /// </summary>
    /// <returns>Список DTO точечных клиентов с их суммарными тратами</returns>
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

        if (!clientSpending.Any()) return Task.FromResult(new List<TopSpenderDto>());

        var maxTotalSpent = clientSpending.Max(x => x.TotalSpent);

        var result = clientSpending
            .Where(x => x.TotalSpent == maxTotalSpent)
            .OrderBy(x => x.Client.Id)
            .Select(x => new TopSpenderDto
            {
                Client = mapper.Map<ClientDto>(x.Client),
                TotalSpent = x.TotalSpent
            })
            .ToList();

        return Task.FromResult(result);
    }
}