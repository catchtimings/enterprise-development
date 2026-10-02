using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для получения аналитических данных и статистики
/// </summary>
/// <param name="analyticsService">Сервис обработки аналитических запросов</param>
/// <param name="logger">Логгер действий контроллера</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает топ-5 ресторанов по наибольшему количеству заказов
    /// </summary>
    /// <returns>Список из 5 лучших ресторанов</returns>
    [HttpGet("top-restaurants")]
    public async Task<ActionResult<List<RestaurantDto>>> GetTopRestaurants()
    {
        logger.LogInformation("Запрос топ-5 ресторанов по заказам");
        var result = await analyticsService.GetTop5RestaurantsByOrderCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Возвращает список заказов с минимальным временем доставки
    /// </summary>
    /// <returns>Заказы с наименьшим значением DeliveryDuration</returns>
    [HttpGet("min-delivery-time-orders")]
    public async Task<ActionResult<List<OrderDto>>> GetMinDeliveryTimeOrders()
    {
        logger.LogInformation("Запрос заказов с минимальным временем доставки");
        var result = await analyticsService.GetOrdersWithMinDeliveryTimeAsync();
        return Ok(result);
    }

    /// <summary>
    /// Возвращает сведения о клиентах указанного ресторана, отсортированные по ФИО
    /// </summary>
    /// <param name="restaurantId">Идентификатор ресторана</param>
    /// <returns>Список клиентов, оформлявших заказы в данном ресторане</returns>
    [HttpGet("restaurant/{restaurantId:int}/clients")]
    public async Task<ActionResult<List<ClientDto>>> GetClientsByRestaurant(int restaurantId)
    {
        logger.LogInformation("Запрос клиентов ресторана с Id {RestaurantId}", restaurantId);
        var result = await analyticsService.GetClientsByRestaurantAsync(restaurantId);
        return Ok(result);
    }

    /// <summary>
    /// Возвращает сводную статистику по категориям блюд за указанный период
    /// </summary>
    /// <param name="startDate">Начальная дата и время периода</param>
    /// <param name="endDate">Конечная дата и время периода</param>
    /// <returns>Агрегированные данные (количество, средняя и общая сумма) по категориям</returns>
    [HttpGet("category-summary")]
    public async Task<ActionResult<List<CategorySummaryDto>>> GetCategorySummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        logger.LogInformation("Запрос сводки по категориям за период с {Start} по {End}", startDate, endDate);
        var result = await analyticsService.GetCategoryOrderSummaryAsync(startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Возвращает информацию о клиентах с наибольшей суммой трат за всё время
    /// </summary>
    /// <returns>Список точечных клиентов с максимальными тратами</returns>
    [HttpGet("top-spenders")]
    public async Task<ActionResult<List<TopSpenderDto>>> GetTopSpenders()
    {
        logger.LogInformation("Запрос клиентов с максимальными суммарными тратами");
        var result = await analyticsService.GetTopSpenderClientsAsync();
        return Ok(result);
    }
}