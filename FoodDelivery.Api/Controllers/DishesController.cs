using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.Dishes;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления блюдами
/// </summary>
/// <param name="dishService">Сервис блюд</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/dishes")]
public class DishesController(IDishService dishService, ILogger<DishesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех блюд
    /// </summary>
    /// <returns>Список блюд</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<DishDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос на получение всех блюд", DateTime.UtcNow);
            var result = await dishService.GetAll();
            logger.LogInformation("{Time} - Успешно получен список блюд - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении списка блюд - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает блюдо по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <returns>Данные блюда или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<DishDto>> GetById(int id)
    {
        var result = await dishService.Get(id);
        if (result is null)
        {
            logger.LogWarning("{Time} - Блюдо с Id {Id} не найдено - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Блюдо не найдено" });
        }
        logger.LogInformation("{Time} - Блюдо с Id {Id} успешно найдено - Response code: 200", DateTime.UtcNow, id);
        return Ok(result);
    }

    /// <summary>
    /// Создает новое блюдо
    /// </summary>
    /// <param name="dto">Данные о блюде</param>
    /// <returns>Созданное блюдо</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<DishDto>> Create([FromBody] DishCreateUpdateDto dto)
    {
        try
        {
            var result = await dishService.Create(dto);
            logger.LogInformation("{Time} - Создано новое блюдо с Id {Id} - Response code: 201", DateTime.UtcNow, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Не найдена категория при создании блюда - Response code: 404", DateTime.UtcNow);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при создании блюда - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Обновляет данные блюда по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <param name="dto">Новые данные о блюде</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] DishCreateUpdateDto dto)
    {
        try
        {
            await dishService.Update(id, dto);
            logger.LogInformation("{Time} - Обновлено блюдо с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Блюдо или категория с Id {Id} не найдены для обновления - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при обновлении блюда с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Удаляет блюдо по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await dishService.Delete(id);
            logger.LogInformation("{Time} - Удалено блюдо с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при удалении блюда с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}