using FoodDelivery.Contracts.Dtos.Dishes;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления блюдами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/dishes")]
public class DishesController(DataSeeder seeder, ILogger<DishesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех блюд
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка блюд");
        return Ok(seeder.Dishes.Select(MapToDto).ToList());
    }

    /// <summary>
    /// Возвращает блюдо по его уникальному идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<DishDto> GetById(int id)
    {
        var dish = seeder.Dishes.FirstOrDefault(d => d.Id == id);
        if (dish == null)
        {
            logger.LogWarning("Блюдо с Id {Id} не найдено", id);
            return NotFound();
        }
        return Ok(MapToDto(dish));
    }

    /// <summary>
    /// Создает новое блюдо
    /// </summary>
    [HttpPost]
    public ActionResult<DishDto> Create([FromBody] DishCreateDto dto)
    {
        var category = seeder.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
        if (category == null)
        {
            logger.LogWarning("Категория с Id {CategoryId} не найдена", dto.CategoryId);
            return NotFound();
        }

        var newId = seeder.Dishes.Any() ? seeder.Dishes.Max(d => d.Id) + 1 : 1;
        var entity = new Dish
        {
            Id = newId,
            Name = dto.Name,
            Weight = dto.Weight,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            Category = category
        };

        seeder.Dishes.Add(entity);
        logger.LogInformation("Успешно создано блюдо с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, MapToDto(entity));
    }

    /// <summary>
    /// Обновляет существующее блюдо
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] DishCreateDto dto)
    {
        var item = seeder.Dishes.FirstOrDefault(d => d.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка обновления несуществующего блюда с Id {Id}", id);
            return NotFound();
        }

        var category = seeder.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
        if (category == null)
        {
            logger.LogWarning("Категория с Id {CategoryId} не найдена", dto.CategoryId);
            return NotFound();
        }

        item.Name = dto.Name;
        item.Weight = dto.Weight;
        item.Price = dto.Price;
        item.CategoryId = dto.CategoryId;
        item.Category = category;

        logger.LogInformation("Успешно обновлено блюдо с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет блюдо по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Dishes.FirstOrDefault(d => d.Id == id);
        if (item != null) seeder.Dishes.Remove(item);
        logger.LogInformation("Успешно удалено блюдо с Id {Id}", id);
        return NoContent();
    }

    private static DishDto MapToDto(Dish dish) => new()
    {
        Id = dish.Id,
        Name = dish.Name,
        Weight = dish.Weight,
        Price = dish.Price,
        CategoryId = dish.CategoryId,
        CategoryName = dish.Category?.Name
    };
}