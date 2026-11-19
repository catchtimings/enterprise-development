using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Репозиторий для получения данных аналитических запросов
/// </summary>
public interface IAnalyticsRepository
{
    /// <summary>
    /// Получает количество заказов для каждого ресторана
    /// </summary>
    public Task<IList<(int RestaurantId, int OrderCount)>> GetRestaurantOrderCounts();

    /// <summary>
    /// Получает заказы с минимальной длительностью доставки
    /// </summary>
    public Task<IList<Order>> GetOrdersWithMinDelivery();

    /// <summary>
    /// Получает уникальных клиентов, сделавших заказ в указанном ресторане
    /// </summary>
    public Task<IList<Client>> GetClientsByRestaurant(int restaurantId);

    /// <summary>
    /// Получает агрегированную статистику по категориям блюд за указанный период
    /// </summary>
    public Task<IList<(string CategoryName, int OrderCount, decimal AverageSum, decimal TotalSum)>> GetCategorySummary(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает общую сумму трат каждого клиента
    /// </summary>
    public Task<IList<(Client Client, decimal TotalSpent)>> GetClientSpendingTotals();
}