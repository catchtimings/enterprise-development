using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Contracts.Dtos.DishCategories;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер справочника категорий блюд
/// </summary>
/// <param name="categoryService">Сервис категорий</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/dish-categories")]
public class DishCategoriesController(
    IDishCategoryService categoryService,
    ILogger<DishCategoriesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех категорий
    /// </summary>
    /// <returns>Список категорий</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<DishCategoryDto>>> GetAll()
    {
        try
        {
            logger.LogInformation("{Time} - Запрос на получение всех категорий блюд", DateTime.UtcNow);
            var result = await categoryService.GetAll();
            logger.LogInformation("{Time} - Успешно получен список категорий блюд - Response code: 200", DateTime.UtcNow);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при получении списка категорий блюд - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Возвращает категорию по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <returns>Данные категории или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<DishCategoryDto>> GetById(int id)
    {
        var result = await categoryService.Get(id);
        if (result is null)
        {
            logger.LogWarning("{Time} - Категория с Id {Id} не найдена - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = "Категория не найдена" });
        }
        logger.LogInformation("{Time} - Категория с Id {Id} успешно найдена - Response code: 200", DateTime.UtcNow, id);
        return Ok(result);
    }

    /// <summary>
    /// Создает новую категорию
    /// </summary>
    /// <param name="dto">Данные о категории</param>
    /// <returns>Созданная категория</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<DishCategoryDto>> Create([FromBody] DishCategoryCreateUpdateDto dto)
    {
        try
        {
            var result = await categoryService.Create(dto);
            logger.LogInformation("{Time} - Создана новая категория с Id {Id} - Response code: 201", DateTime.UtcNow, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при создании категории - Response code: 500", DateTime.UtcNow);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Обновляет данные категории по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <param name="dto">Новые данные о категории</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] DishCategoryCreateUpdateDto dto)
    {
        try
        {
            await categoryService.Update(id, dto);
            logger.LogInformation("{Time} - Обновлена категория с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "{Time} - Категория с Id {Id} не найдена для обновления - Response code: 404", DateTime.UtcNow, id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при обновлении категории с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Удаляет категорию по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await categoryService.Delete(id);
            logger.LogInformation("{Time} - Удалена категория с Id {Id} - Response code: 204", DateTime.UtcNow, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Time} - Ошибка при удалении категории с Id {Id} - Response code: 500", DateTime.UtcNow, id);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }
}