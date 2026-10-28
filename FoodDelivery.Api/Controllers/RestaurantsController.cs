using FoodDelivery.Contracts.Dtos.Restaurants;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления ресторанами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(DataSeeder seeder, ILogger<RestaurantsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка ресторанов");
        return Ok(seeder.Restaurants.Select(MapToDto).ToList());
    }

    /// <summary>
    /// Возвращает ресторан по его уникальному идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<RestaurantDto> GetById(int id)
    {
        var restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant == null)
        {
            logger.LogWarning("Ресторан с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(MapToDto(restaurant));
    }

    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    [HttpPost]
    public ActionResult<RestaurantDto> Create([FromBody] RestaurantCreateDto dto)
    {
        var newId = seeder.Restaurants.Any() ? seeder.Restaurants.Max(r => r.Id) + 1 : 1;
        var entity = new Restaurant
        {
            Id = newId,
            Name = dto.Name,
            Address = dto.Address,
            Rating = dto.Rating,
            OpeningTime = dto.OpeningTime,
            ClosingTime = dto.ClosingTime
        };

        seeder.Restaurants.Add(entity);
        logger.LogInformation("Успешно создан ресторан с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, MapToDto(entity));
    }

    /// <summary>
    /// Обновляет существующий ресторан
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] RestaurantCreateDto dto)
    {
        var item = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка обновления несуществующего ресторана с Id {Id}", id);
            return NotFound();
        }

        item.Name = dto.Name;
        item.Address = dto.Address;
        item.Rating = dto.Rating;
        item.OpeningTime = dto.OpeningTime;
        item.ClosingTime = dto.ClosingTime;

        logger.LogInformation("Успешно обновлен ресторан с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет ресторан по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Restaurants.FirstOrDefault(r => r.Id == id);
        if (item != null) seeder.Restaurants.Remove(item);
        logger.LogInformation("Успешно удален ресторан с Id {Id}", id);
        return NoContent();
    }

    private static RestaurantDto MapToDto(Restaurant restaurant) => new()
    {
        Id = restaurant.Id,
        Name = restaurant.Name,
        Address = restaurant.Address,
        Rating = restaurant.Rating,
        OpeningTime = restaurant.OpeningTime,
        ClosingTime = restaurant.ClosingTime
    };
}