using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления блюдами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="mapper">Маппер объектов</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/dishes")]
public class DishesController(DataSeeder seeder, IMapper mapper, ILogger<DishesController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех блюд
    /// </summary>
    /// <returns>Коллекция DTO блюд</returns>
    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка блюд");
        return Ok(mapper.Map<IEnumerable<DishDto>>(seeder.Dishes));
    }

    /// <summary>
    /// Возвращает блюдо по его уникальному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <returns>DTO блюда или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<DishDto> GetById(int id)
    {
        var dish = seeder.Dishes.FirstOrDefault(d => d.Id == id);
        if (dish == null)
        {
            logger.LogWarning("Блюдо с Id {Id} не найдено", id);
            return NotFound();
        }
        return Ok(mapper.Map<DishDto>(dish));
    }

    /// <summary>
    /// Создает новое блюдо
    /// </summary>
    /// <param name="dto">Модель данных для создания блюда</param>
    /// <returns>Созданный объект блюда со статусом 201 Created</returns>
    [HttpPost]
    public ActionResult<DishDto> Create([FromBody] DishCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = seeder.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
        if (category == null)
        {
            logger.LogWarning("Категория с Id {CategoryId} не найдена", dto.CategoryId);
            return NotFound();
        }

        var newId = seeder.Dishes.Any() ? seeder.Dishes.Max(d => d.Id) + 1 : 1;
        var entity = mapper.Map<Dish>(dto);
        entity.Id = newId;
        entity.Category = category;

        seeder.Dishes.Add(entity);
        logger.LogInformation("Успешно создано блюдо с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, mapper.Map<DishDto>(entity));
    }

    /// <summary>
    /// Обновляет существующее блюдо
    /// </summary>
    /// <param name="id">Идентификатор обновляемого блюда</param>
    /// <param name="dto">Модель данных для обновления</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] DishCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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

        mapper.Map(dto, item);
        item.Category = category;
        logger.LogInformation("Успешно обновлено блюдо с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет блюдо по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор удаляемого блюда</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Dishes.FirstOrDefault(d => d.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка удаления несуществующего блюда с Id {Id}", id);
            return NotFound();
        }

        seeder.Dishes.Remove(item);
        logger.LogInformation("Успешно удалено блюдо с Id {Id}", id);
        return NoContent();
    }
}