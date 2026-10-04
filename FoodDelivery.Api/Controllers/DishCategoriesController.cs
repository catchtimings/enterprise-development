using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер справочника категорий блюд
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="mapper">Маппер объектов</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/dish-categories")]
public class DishCategoriesController(DataSeeder seeder, IMapper mapper, ILogger<DishCategoriesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех категорий блюд
    /// </summary>
    /// <returns>Коллекция DTO категорий</returns>
    [HttpGet]
    public ActionResult<IEnumerable<DishCategoryDto>> GetAll()
    {
        logger.LogInformation("Получение списка категорий блюд");
        return Ok(mapper.Map<IEnumerable<DishCategoryDto>>(seeder.Categories));
    }

    /// <summary>
    /// Возвращает категорию по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <returns>DTO категории или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<DishCategoryDto> GetById(int id)
    {
        var category = seeder.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            logger.LogWarning("Категория с Id {Id} не найдена", id);
            return NotFound();
        }
        return Ok(mapper.Map<DishCategoryDto>(category));
    }
}