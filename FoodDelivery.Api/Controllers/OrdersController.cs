using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления заказами
/// </summary>
/// <param name="orderService">Сервис заказов</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/orders")]
public class OrdersController(OrderService orderService, ILogger<OrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заказов
    /// </summary>
    /// <returns>Список заказов</returns>
    [HttpGet]
    public async Task<ActionResult<IList<OrderDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос на получение всех заказов", DateTime.UtcNow);
            var result = await orderService.GetAll();
            logger.LogInformation("{Time} - Успешно получен список заказов - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении списка заказов - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Данные заказа или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var result = await orderService.Get(id);
        if (result is null)
        {
            logger.LogWarning("{Time} - Заказ с Id {Id} не найден - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Заказ не найден" });
        }
        logger.LogInformation("{Time} - Заказ с Id {Id} успешно найден - Response code: 200", DateTime.UtcNow, id);
        return Ok(result);
    }

    /// <summary>
    /// Создает новый заказ
    /// </summary>
    /// <param name="dto">Данные о заказе</param>
    /// <returns>Созданный заказ</returns>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] OrderCreateUpdateDto dto)
    {
        try
        {
            var result = await orderService.Create(dto);
            logger.LogInformation("{Time} - Создан новый заказ с Id {Id} - Response code: 201", DateTime.UtcNow, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Не найдены связанные сущности - Response code: 404", DateTime.UtcNow);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "{Time} - Некорректные данные - Response code: 400", DateTime.UtcNow);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при создании заказа - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Обновляет данные заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="dto">Новые данные о заказе</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] OrderCreateUpdateDto dto)
    {
        try
        {
            await orderService.Update(id, dto);
            logger.LogInformation("{Time} - Обновлен заказ с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Заказ или связанные сущности с Id {Id} не найдены - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "{Time} - Некорректные данные для обновления заказа с Id {Id} - Response code: 400", DateTime.UtcNow, id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при обновлении заказа с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Удаляет заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await orderService.Delete(id);
            logger.LogInformation("{Time} - Удален заказ с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при удалении заказа с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает позиции указанного заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Список позиций заказа</returns>
    [HttpGet("{id:int}/items")]
    public async Task<ActionResult<IList<OrderItemDto>>> GetItems(int id)
    {
        try
        {
            var result = await orderService.GetItems(id);
            logger.LogInformation("{Time} - Успешно получены позиции заказа с Id {Id} - Response code: 200", DateTime.UtcNow, id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Заказ с Id {Id} не найден - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении позиций заказа с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}