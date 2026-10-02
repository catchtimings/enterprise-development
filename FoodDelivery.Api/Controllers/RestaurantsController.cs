using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления ресторанами (CRUD)
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="mapper">Маппер объектов</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(DataSeeder seeder, IMapper mapper, ILogger<RestaurantsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    /// <returns>Коллекция DTO ресторанов</returns>
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка ресторанов");
        return Ok(mapper.Map<IEnumerable<RestaurantDto>>(seeder.Restaurants));
    }

    /// <summary>
    /// Возвращает ресторан по его уникальному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор ресторана</param>
    /// <returns>DTO ресторана или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<RestaurantDto> GetById(int id)
    {
        var restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant == null)
        {
            logger.LogWarning("Ресторан с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(mapper.Map<RestaurantDto>(restaurant));
    }

    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    /// <param name="dto">Модель данных для создания ресторана</param>
    /// <returns>Созданный объект ресторана со статусом 201 Created</returns>
    [HttpPost]
    public ActionResult<RestaurantDto> Create([FromBody] RestaurantCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newId = seeder.Restaurants.Any() ? seeder.Restaurants.Max(r => r.Id) + 1 : 1;
        var entity = mapper.Map<Restaurant>(dto);
        entity.Id = newId;

        seeder.Restaurants.Add(entity);
        logger.LogInformation("Успешно создан ресторан с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, mapper.Map<RestaurantDto>(entity));
    }

    /// <summary>
    /// Удаляет ресторан по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор удаляемого ресторана</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка удаления несуществующего ресторана с Id {Id}", id);
            return NotFound();
        }

        seeder.Restaurants.Remove(item);
        logger.LogInformation("Успешно удален ресторан с Id {Id}", id);
        return NoContent();
    }
}