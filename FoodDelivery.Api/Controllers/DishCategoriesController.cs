using FoodDelivery.Contracts.Dtos.DishCategories;
using FoodDelivery.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер справочника категорий блюд
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/dish-categories")]
public class DishCategoriesController(DataSeeder seeder, ILogger<DishCategoriesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех категорий блюд
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<DishCategoryDto>> GetAll()
    {
        logger.LogInformation("Получение списка категорий блюд");
        return Ok(seeder.Categories.Select(MapToDto).ToList());
    }

    /// <summary>
    /// Возвращает категорию по идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<DishCategoryDto> GetById(int id)
    {
        var category = seeder.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            logger.LogWarning("Категория с Id {Id} не найдена", id);
            return NotFound();
        }
        return Ok(MapToDto(category));
    }

    private static DishCategoryDto MapToDto(Domain.Entities.DishCategory category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };
}