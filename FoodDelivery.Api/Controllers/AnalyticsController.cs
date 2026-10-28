using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;
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
    [HttpGet("top-restaurants")]
    public async Task<ActionResult<List<RestaurantWithOrderCountDto>>> GetTopRestaurants()
    {
        logger.LogInformation("Запрос топ-5 ресторанов по заказам");
        var result = await analyticsService.GetTop5RestaurantsByOrderCountAsync();
        return Ok(result);
    }

    /// <summary>
    /// Возвращает заказы с минимальным временем доставки и само это время
    /// </summary>
    [HttpGet("min-delivery-time-orders")]
    public async Task<ActionResult<MinDeliveryTimeOrdersDto>> GetMinDeliveryTimeOrders()
    {
        logger.LogInformation("Запрос заказов с минимальным временем доставки");
        var result = await analyticsService.GetOrdersWithMinDeliveryTimeAsync();
        return Ok(result);
    }

    /// <summary>
    /// Возвращает клиентов указанного ресторана, отсортированных по ФИО
    /// </summary>
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
    [HttpGet("category-summary")]
    public async Task<ActionResult<List<CategorySummaryDto>>> GetCategorySummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        logger.LogInformation("Запрос сводки по категориям за период с {Start} по {End}", startDate, endDate);
        var result = await analyticsService.GetCategoryOrderSummaryAsync(startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Возвращает клиентов с наибольшей суммой трат за всё время
    /// </summary>
    [HttpGet("top-spenders")]
    public async Task<ActionResult<List<TopSpenderDto>>> GetTopSpenders()
    {
        logger.LogInformation("Запрос клиентов с максимальными суммарными тратами");
        var result = await analyticsService.GetTopSpenderClientsAsync();
        return Ok(result);
    }
}