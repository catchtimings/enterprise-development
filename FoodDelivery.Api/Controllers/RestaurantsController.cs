using FoodDelivery.Application.Services;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.Restaurants;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления ресторанами
/// </summary>
/// <param name="restaurantService">Сервис ресторанов</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantService restaurantService, ILogger<RestaurantsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    /// <returns>Список ресторанов</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<RestaurantDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос на получение всех ресторанов", DateTime.UtcNow);
            var result = await restaurantService.GetAll();
            logger.LogInformation("{Time} - Успешно получен список ресторанов - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении списка ресторанов - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает ресторан по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор ресторана</param>
    /// <returns>Данные ресторана или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var result = await restaurantService.Get(id);
        if (result is null)
        {
            logger.LogWarning("{Time} - Ресторан с Id {Id} не найден - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Ресторан не найден" });
        }
        logger.LogInformation("{Time} - Ресторан с Id {Id} успешно найден - Response code: 200", DateTime.UtcNow, id);
        return Ok(result);
    }

    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    /// <param name="dto">Данные о ресторане</param>
    /// <returns>Созданный ресторан</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<RestaurantDto>> Create([FromBody] RestaurantCreateUpdateDto dto)
    {
        try
        {
            var result = await restaurantService.Create(dto);
            logger.LogInformation("{Time} - Создан новый ресторан с Id {Id} - Response code: 201", DateTime.UtcNow, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при создании ресторана - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Обновляет данные ресторана по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор ресторана</param>
    /// <param name="dto">Новые данные о ресторане</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] RestaurantCreateUpdateDto dto)
    {
        try
        {
            await restaurantService.Update(id, dto);
            logger.LogInformation("{Time} - Обновлен ресторан с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Ресторан с Id {Id} не найден для обновления - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при обновлении ресторана с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Удаляет ресторан по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор ресторана</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await restaurantService.Delete(id);
            logger.LogInformation("{Time} - Удален ресторан с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при удалении ресторана с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}