using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для получения аналитических данных
/// </summary>
/// <param name="analyticsService">Сервис аналитики</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Возвращает топ-5 ресторанов по количеству заказов
    /// </summary>
    [HttpGet("top-restaurants")]
    public async Task<ActionResult<IEnumerable<RestaurantWithOrderCountDto>>> GetTopRestaurants()
    {
        return Ok(await analyticsService.GetTop5Restaurants());
    }

    /// <summary>
    /// Возвращает заказы с минимальным временем доставки
    /// </summary>
    [HttpGet("min-delivery-time-orders")]
    public async Task<ActionResult<MinDeliveryTimeOrdersDto>> GetMinDeliveryTimeOrders()
    {
        return Ok(await analyticsService.GetMinDeliveryTimeOrders());
    }

    /// <summary>
    /// Возвращает клиентов указанного ресторана
    /// </summary>
    [HttpGet("restaurant/{restaurantId:int}/clients")]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByRestaurant(int restaurantId)
    {
        return Ok(await analyticsService.GetClientsByRestaurant(restaurantId));
    }

    /// <summary>
    /// Возвращает сводную статистику по категориям за период
    /// </summary>
    [HttpGet("category-summary")]
    public async Task<ActionResult<IEnumerable<CategorySummaryDto>>> GetCategorySummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        return Ok(await analyticsService.GetCategorySummary(startDate, endDate));
    }

    /// <summary>
    /// Возвращает клиентов с максимальной суммой трат
    /// </summary>
    [HttpGet("top-spenders")]
    public async Task<ActionResult<IEnumerable<TopSpenderDto>>> GetTopSpenders()
    {
        return Ok(await analyticsService.GetTopSpenders());
    }
}