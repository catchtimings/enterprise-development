using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления клиентами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="mapper">Маппер объектов</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(DataSeeder seeder, IMapper mapper, ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех клиентов
    /// </summary>
    /// <returns>Коллекция DTO клиентов</returns>
    [HttpGet]
    public ActionResult<IEnumerable<ClientDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка клиентов");
        return Ok(mapper.Map<IEnumerable<ClientDto>>(seeder.Clients));
    }

    /// <summary>
    /// Возвращает клиента по его уникальному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>DTO клиента или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<ClientDto> GetById(int id)
    {
        var client = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            logger.LogWarning("Клиент с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(mapper.Map<ClientDto>(client));
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="dto">Модель данных для создания клиента</param>
    /// <returns>Созданный объект клиента со статусом 201 Created</returns>
    [HttpPost]
    public ActionResult<ClientDto> Create([FromBody] ClientCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newId = seeder.Clients.Any() ? seeder.Clients.Max(c => c.Id) + 1 : 1;
        var entity = mapper.Map<Client>(dto);
        entity.Id = newId;

        seeder.Clients.Add(entity);
        logger.LogInformation("Успешно создан клиент с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, mapper.Map<ClientDto>(entity));
    }

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    /// <param name="id">Идентификатор обновляемого клиента</param>
    /// <param name="dto">Модель данных для обновления</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] ClientCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var item = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка обновления несуществующего клиента с Id {Id}", id);
            return NotFound();
        }

        mapper.Map(dto, item);
        logger.LogInformation("Успешно обновлен клиент с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор удаляемого клиента</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка удаления несуществующего клиента с Id {Id}", id);
            return NotFound();
        }

        seeder.Clients.Remove(item);
        logger.LogInformation("Успешно удален клиент с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает список заказов указанного клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Коллекция DTO заказов клиента</returns>
    [HttpGet("{id:int}/orders")]
    public ActionResult<IEnumerable<OrderDto>> GetOrders(int id)
    {
        var client = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            logger.LogWarning("Клиент с Id {Id} не найден", id);
            return NotFound();
        }

        var orders = seeder.Orders.Where(o => o.ClientId == id).ToList();
        return Ok(mapper.Map<IEnumerable<OrderDto>>(orders));
    }
}