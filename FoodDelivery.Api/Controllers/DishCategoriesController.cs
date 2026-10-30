using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.DishCategories;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер справочника категорий блюд
/// </summary>
/// <param name="categoryService">Сервис категорий</param>
[ApiController]
[Route("api/dish-categories")]
public class DishCategoriesController(DishCategoryService categoryService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех категорий
    /// </summary>
    /// <returns>Список категорий</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishCategoryDto>>> GetAll()
    {
        return Ok(await categoryService.GetAll());
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
        return result is null ? NotFound() : Ok(result);
    }
}