using FoodDelivery.Application.Services;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.Analytics;
using FoodDelivery.Contracts.Dtos.Clients;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для получения аналитических данных
/// </summary>
/// <param name="analyticsService">Сервис аналитики</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает топ-5 ресторанов по количеству заказов
    /// </summary>
    /// <returns>Список ресторанов с количеством заказов</returns>
    [HttpGet("top-restaurants")]
    public async Task<ActionResult<IList<RestaurantWithOrderCountDto>>> GetTop5RestaurantsByOrderCount()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос топ-5 ресторанов по заказам", DateTime.UtcNow);
            var result = await analyticsService.GetTop5RestaurantsByOrderCount();
            logger.LogInformation("{Time} - Успешно получен топ-5 ресторанов - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении топа ресторанов - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает заказы с минимальным временем доставки
    /// </summary>
    /// <returns>Минимальное время доставки и список заказов</returns>
    [HttpGet("min-delivery-time-orders")]
    public async Task<ActionResult<MinDeliveryTimeOrdersDto>> GetOrdersWithMinDelivery()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос заказов с минимальным временем доставки", DateTime.UtcNow);
            var result = await analyticsService.GetOrdersWithMinDelivery();
            logger.LogInformation("{Time} - Успешно получены заказы с минимальным временем доставки - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении заказов с минимальным временем доставки - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает клиентов указанного ресторана
    /// </summary>
    /// <param name="restaurantId">Идентификатор ресторана</param>
    /// <returns>Список клиентов ресторана</returns>
    [HttpGet("restaurant/{restaurantId:int}/clients")]
    public async Task<ActionResult<IList<ClientDto>>> GetClientsByRestaurant(int restaurantId)
    {
        try
        {
            logger.LogInformation("{Time} - Запрос клиентов ресторана с Id {RestaurantId}", DateTime.UtcNow, restaurantId);
            var result = await analyticsService.GetClientsByRestaurant(restaurantId);
            logger.LogInformation("{Time} - Успешно получены клиенты ресторана с Id {RestaurantId} - Response code: 200", DateTime.UtcNow, restaurantId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении клиентов ресторана с Id {RestaurantId} - Response code: 500", DateTime.UtcNow, restaurantId);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает сводную статистику по категориям за период
    /// </summary>
    /// <param name="startDate">Начальная дата периода</param>
    /// <param name="endDate">Конечная дата периода</param>
    /// <returns>Список категорий с агрегированной статистикой</returns>
    [HttpGet("category-summary")]
    public async Task<ActionResult<IList<CategorySummaryDto>>> GetCategoryOrderSummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            logger.LogInformation("{Time} - Запрос сводки по категориям за период с {Start} по {End}", DateTime.UtcNow, startDate, endDate);
            var result = await analyticsService.GetCategoryOrderSummary(startDate, endDate);
            logger.LogInformation("{Time} - Успешно получена сводка по категориям - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении сводки по категориям - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает клиентов с максимальной суммой трат
    /// </summary>
    /// <returns>Список клиентов и их общая сумма трат</returns>
    [HttpGet("top-spenders")]
    public async Task<ActionResult<IList<TopSpenderDto>>> GetTopSpenderClients()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос клиентов с максимальными тратами", DateTime.UtcNow);
            var result = await analyticsService.GetTopSpenderClients();
            logger.LogInformation("{Time} - Успешно получены клиенты с максимальными тратами - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении клиентов с максимальными тратами - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}