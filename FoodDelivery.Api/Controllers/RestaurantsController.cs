using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Restaurants;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления ресторанами
/// </summary>
/// <param name="restaurantService">Сервис ресторанов</param>
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(RestaurantService restaurantService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех ресторанов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        return Ok(await restaurantService.GetAll());
    }

    /// <summary>
    /// Возвращает ресторан по идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var result = await restaurantService.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Создает новый ресторан
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RestaurantDto>> Create([FromBody] RestaurantCreateDto dto)
    {
        var result = await restaurantService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновляет данные ресторана
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] RestaurantCreateDto dto)
    {
        await restaurantService.Update(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Удаляет ресторан
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await restaurantService.Delete(id);
        return NoContent();
    }
}