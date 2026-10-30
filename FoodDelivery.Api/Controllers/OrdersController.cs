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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        return Ok(await orderService.GetAll());
    }

    /// <summary>
    /// Возвращает заказ по идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var result = await orderService.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Создает новый заказ
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] OrderCreateDto dto)
    {
        var result = await orderService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновляет данные заказа
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] OrderCreateDto dto)
    {
        await orderService.Update(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заказ
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await orderService.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает позиции указанного заказа
    /// </summary>
    [HttpGet("{id:int}/items")]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetItems(int id)
    {
        return Ok(await orderService.GetItems(id));
    }
}