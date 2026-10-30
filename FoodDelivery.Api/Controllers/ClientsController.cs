using FoodDelivery.Application.Services;
using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Contracts.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер клиентов
/// </summary>
/// <param name="clientService">Сервис клиентов</param>
/// <param name="orderService">Сервис заказов</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(ClientService clientService, OrderService orderService) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех клиентов
    /// </summary>
    /// <returns>Список клиентов</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll()
    {
        return Ok(await clientService.GetAll());
    }

    /// <summary>
    /// Возвращает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Данные клиента или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var result = await clientService.Get(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="dto">Данные о клиенте</param>
    /// <returns>Созданный клиент</returns>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create([FromBody] ClientCreateDto dto)
    {
        var result = await clientService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновляет данные клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="dto">Новые данные о клиенте</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] ClientCreateDto dto)
    {
        await clientService.Update(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Статус 204 No Content</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await clientService.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает заказы указанного клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Список заказов клиента</returns>
    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(int id)
    {
        var client = await clientService.Get(id);
        if (client is null) return NotFound();

        var allOrders = await orderService.GetAll();
        var clientOrders = allOrders.Where(o => o.ClientId == id);
        return Ok(clientOrders);
    }
}