using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Dishes;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления блюдами
/// </summary>
/// <param name="dishService">Сервис блюд</param>
[ApiController]
[Route("api/dishes")]
public class DishesController(DishService dishService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех блюд
    /// </summary>
    /// <returns>Список блюд</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll()
    {
        return Ok(await dishService.GetAll());
    }

    /// <summary>
    /// Возвращает блюдо по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <returns>Данные блюда или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DishDto>> GetById(int id)
    {
        var result = await dishService.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Создает новое блюдо
    /// </summary>
    /// <param name="dto">Данные о блюде</param>
    /// <returns>Созданное блюдо</returns>
    [HttpPost]
    public async Task<ActionResult<DishDto>> Create([FromBody] DishCreateDto dto)
    {
        var result = await dishService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновляет данные блюда по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <param name="dto">Новые данные о блюде</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] DishCreateDto dto)
    {
        await dishService.Update(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Удаляет блюдо по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор блюда</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await dishService.Delete(id);
        return NoContent();
    }
}