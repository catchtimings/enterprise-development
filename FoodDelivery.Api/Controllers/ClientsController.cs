using FoodDelivery.Application.Services;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Contracts.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер клиентов
/// </summary>
/// <param name="clientService">Сервис клиентов</param>
/// <param name="orderService">Сервис заказов</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(
    IClientService clientService, 
    IOrderService orderService, 
    ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех клиентов
    /// </summary>
    /// <returns>Список клиентов</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<ClientDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос на получение всех клиентов", DateTime.UtcNow);
            var result = await clientService.GetAll();
            logger.LogInformation("{Time} - Успешно получен список клиентов - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении списка клиентов - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Данные клиента или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var result = await clientService.Get(id);
        if (result is null)
        {
            logger.LogWarning("{Time} - Клиент с Id {Id} не найден - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Клиент не найден" });
        }
        logger.LogInformation("{Time} - Клиент с Id {Id} успешно найден - Response code: 200", DateTime.UtcNow, id);
        return Ok(result);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="dto">Данные о клиенте</param>
    /// <returns>Созданный клиент</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ClientDto>> Create([FromBody] ClientCreateUpdateDto dto)
    {
        try
        {
            var result = await clientService.Create(dto);
            logger.LogInformation("{Time} - Создан новый клиент с Id {Id} - Response code: 201", DateTime.UtcNow, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при создании клиента - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Обновляет данные клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="dto">Новые данные о клиенте</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] ClientCreateUpdateDto dto)
    {
        try
        {
            await clientService.Update(id, dto);
            logger.LogInformation("{Time} - Обновлен клиент с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Клиент с Id {Id} не найден для обновления - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при обновлении клиента с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await clientService.Delete(id);
            logger.LogInformation("{Time} - Удален клиент с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при удалении клиента с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает заказы указанного клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Список заказов клиента</returns>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<OrderDto>>> GetOrders(int id)
    {
        var client = await clientService.Get(id);
        if (client is null)
        {
            logger.LogWarning("{Time} - Клиент с Id {Id} не найден - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Клиент не найден" });
        }

        try
        {
            var allOrders = await orderService.GetAll();
            var clientOrders = allOrders.Where(o => o.ClientId == id).ToList();
            logger.LogInformation("{Time} - Успешно получены заказы клиента с Id {Id} - Response code: 200", DateTime.UtcNow, id);
            return Ok(clientOrders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении заказов клиента с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}