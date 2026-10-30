using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления заказами
/// </summary>
/// <param name="orderService">Сервис заказов</param>
[ApiController]
[Route("api/orders")]
public class OrdersController(OrderService orderService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заказов
    /// </summary>
    /// <returns>Список заказов</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        return Ok(await orderService.GetAll());
    }

    /// <summary>
    /// Возвращает заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Данные заказа или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var result = await orderService.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Создает новый заказ
    /// </summary>
    /// <param name="dto">Данные о заказе</param>
    /// <returns>Созданный заказ</returns>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] OrderCreateDto dto)
    {
        var result = await orderService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновляет данные заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="dto">Новые данные о заказе</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] OrderCreateDto dto)
    {
        await orderService.Update(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await orderService.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает позиции указанного заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Список позиций заказа</returns>
    [HttpGet("{id:int}/items")]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetItems(int id)
    {
        return Ok(await orderService.GetItems(id));
    }
}