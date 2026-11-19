using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис аналитики
/// </summary>
/// <param name="analyticsRepository">Репозиторий аналитических данных</param>
/// <param name="restaurantRepository">Репозиторий ресторанов</param>
public class AnalyticsService(
    IAnalyticsRepository analyticsRepository,
    IRestaurantRepository restaurantRepository) : IAnalyticsService
{
    /// <inheritdoc />
    public async Task<IList<RestaurantWithOrderCountDto>> GetTop5RestaurantsByOrderCount()
    {
        var counts = await analyticsRepository.GetRestaurantOrderCounts();
        var result = new List<RestaurantWithOrderCountDto>();

        var top5 = counts
            .OrderByDescending(x => x.OrderCount)
            .ThenBy(x => x.RestaurantId)
            .Take(5);

        foreach (var (restaurantId, orderCount) in top5)
        {
            var restaurant = await restaurantRepository.GetById(restaurantId);
            if (restaurant != null)
                result.Add(new RestaurantWithOrderCountDto(RestaurantMapper.Map(restaurant), orderCount));
        }
        return result;
    }

    /// <inheritdoc />
    public async Task<MinDeliveryTimeOrdersDto> GetOrdersWithMinDelivery()
    {
        var orders = await analyticsRepository.GetOrdersWithMinDelivery();
        if (!orders.Any()) throw new KeyNotFoundException("Нет доставленных заказов");

        var minDuration = orders.Min(o => o.DeliveryDuration!.Value);
        return new MinDeliveryTimeOrdersDto(minDuration, orders.Select(OrderMapper.Map).ToList());
    }

    /// <inheritdoc />
    public async Task<IList<ClientDto>> GetClientsByRestaurant(int restaurantId)
    {
        var restaurant = await restaurantRepository.GetById(restaurantId);
        if (restaurant == null) throw new KeyNotFoundException($"Ресторан {restaurantId} не найден");

        var clients = await analyticsRepository.GetClientsByRestaurant(restaurantId);
        var sortedClients = clients.OrderBy(c => c.FullName).ToList();
        return ClientMapper.Map(sortedClients);
    }

    /// <inheritdoc />
    public async Task<IList<CategorySummaryDto>> GetCategoryOrderSummary(DateTime startDate, DateTime endDate)
    {
        var data = await analyticsRepository.GetCategorySummary(startDate, endDate);
        return data.Select(x => new CategorySummaryDto(x.CategoryName, x.OrderCount, x.AverageSum, x.TotalSum)).ToList();
    }

    /// <inheritdoc />
    public async Task<IList<TopSpenderDto>> GetTopSpenderClients()
    {
        var data = await analyticsRepository.GetClientSpendingTotals();
        if (!data.Any()) return [];

        var maxSpent = data.Max(x => x.TotalSpent);

        return data
            .Where(x => x.TotalSpent == maxSpent)
            .OrderBy(x => x.Client.Id)
            .Select(x => new TopSpenderDto(ClientMapper.Map(x.Client), x.TotalSpent))
            .ToList();
    }
}