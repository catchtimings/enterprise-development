using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;

namespace FoodDelivery.Application.Interfaces.Services;

/// <summary>
/// Интерфейс для сервиса аналитики
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Возвращает пять ресторанов с наибольшим количеством заказов
    /// </summary>
    public Task<IList<RestaurantWithOrderCountDto>> GetTop5RestaurantsByOrderCount();

    /// <summary>
    /// Возвращает заказы с минимальным временем доставки
    /// </summary>
    public Task<MinDeliveryTimeOrdersDto> GetOrdersWithMinDelivery();

    /// <summary>
    /// Возвращает клиентов выбранного ресторана, отсортированных по ФИО
    /// </summary>
    public Task<IList<ClientDto>> GetClientsByRestaurant(int restaurantId);

    /// <summary>
    /// Возвращает агрегированную статистику по категориям за заданный период
    /// </summary>
    public Task<IList<CategorySummaryDto>> GetCategoryOrderSummary(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Возвращает всех клиентов с максимальной общей суммой заказов
    /// </summary>
    public Task<IList<TopSpenderDto>> GetTopSpenderClients();
}