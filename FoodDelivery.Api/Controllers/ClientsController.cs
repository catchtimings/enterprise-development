using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления клиентами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(DataSeeder seeder, ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех клиентов
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<ClientDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка клиентов");
        return Ok(seeder.Clients.Select(MapToDto).ToList());
    }

    /// <summary>
    /// Возвращает клиента по его уникальному идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<ClientDto> GetById(int id)
    {
        var client = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            logger.LogWarning("Клиент с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(MapToDto(client));
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    [HttpPost]
    public ActionResult<ClientDto> Create([FromBody] ClientCreateDto dto)
    {
        var newId = seeder.Clients.Any() ? seeder.Clients.Max(c => c.Id) + 1 : 1;
        var entity = new Client
        {
            Id = newId,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            DeliveryAddress = dto.DeliveryAddress
        };

        seeder.Clients.Add(entity);
        logger.LogInformation("Успешно создан клиент с Id {Id}", newId);

        return CreatedAtAction(nameof(GetById), new { id = newId }, MapToDto(entity));
    }

    /// <summary>
    /// Обновляет существующего клиента
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] ClientCreateDto dto)
    {
        var item = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка обновления несуществующего клиента с Id {Id}", id);
            return NotFound();
        }

        item.FullName = dto.FullName;
        item.PhoneNumber = dto.PhoneNumber;
        item.DeliveryAddress = dto.DeliveryAddress;

        logger.LogInformation("Успешно обновлен клиент с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (item != null) seeder.Clients.Remove(item);
        logger.LogInformation("Успешно удален клиент с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает список заказов указанного клиента
    /// </summary>
    [HttpGet("{id:int}/orders")]
    public ActionResult<IEnumerable<OrderDto>> GetOrders(int id)
    {
        var client = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            logger.LogWarning("Клиент с Id {Id} не найден", id);
            return NotFound();
        }

        var orders = seeder.Orders
            .Where(o => o.ClientId == id)
            .Select(MapOrderToDto)
            .ToList();

        return Ok(orders);
    }

    private static ClientDto MapToDto(Client client) => new()
    {
        Id = client.Id,
        FullName = client.FullName,
        PhoneNumber = client.PhoneNumber,
        DeliveryAddress = client.DeliveryAddress
    };

    private static OrderDto MapOrderToDto(Order order) => new()
    {
        Id = order.Id,
        ClientId = order.ClientId,
        ClientName = order.Client?.FullName,
        RestaurantId = order.RestaurantId,
        RestaurantName = order.Restaurant?.Name,
        CreatedAt = order.CreatedAt,
        DeliveredAt = order.DeliveredAt,
        TotalAmount = order.TotalAmount,
        DeliveryDuration = order.DeliveryDuration,
        Items = order.Items.Select(MapOrderItemToDto).ToList()
    };

    private static OrderItemDto MapOrderItemToDto(OrderItem item) => new()
    {
        Id = item.Id,
        OrderId = item.OrderId,
        DishId = item.DishId,
        DishName = item.Dish?.Name,
        Quantity = item.Quantity,
        PriceAtOrder = item.PriceAtOrder
    };
}