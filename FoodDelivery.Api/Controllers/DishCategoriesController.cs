using FoodDelivery.Application.Services;
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
public class DishCategoriesController(DishCategoryService categoryService, ILogger<DishCategoriesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех категорий
    /// </summary>
    /// <returns>Список категорий</returns>
    [HttpGet]
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
}