using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления заказами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="mapper">Маппер объектов</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/orders")]
public class OrdersController(DataSeeder seeder, IMapper mapper, ILogger<OrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заказов
    /// </summary>
    /// <returns>Коллекция DTO заказов</returns>
    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка заказов");
        return Ok(mapper.Map<IEnumerable<OrderDto>>(seeder.Orders));
    }

    /// <summary>
    /// Возвращает заказ по его уникальному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>DTO заказа или 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<OrderDto> GetById(int id)
    {
        var order = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            logger.LogWarning("Заказ с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(mapper.Map<OrderDto>(order));
    }

    /// <summary>
    /// Создает новый заказ
    /// </summary>
    /// <param name="dto">Модель данных для создания заказа</param>
    /// <returns>Созданный объект заказа со статусом 201 Created</returns>
    [HttpPost]
    public ActionResult<OrderDto> Create([FromBody] OrderCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var client = seeder.Clients.FirstOrDefault(c => c.Id == dto.ClientId);
        var restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId);
        if (client == null || restaurant == null)
        {
            logger.LogWarning("Клиент или ресторан не найдены");
            return NotFound();
        }

        var newOrderId = seeder.Orders.Any() ? seeder.Orders.Max(o => o.Id) + 1 : 1;
        var entity = mapper.Map<Order>(dto);
        entity.Id = newOrderId;
        entity.Client = client;
        entity.Restaurant = restaurant;

        var newItemId = seeder.Orders
            .SelectMany(o => o.Items)
            .Any()
            ? seeder.Orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        foreach (var orderItem in entity.Items)
        {
            var dish = seeder.Dishes.FirstOrDefault(d => d.Id == orderItem.DishId);
            if (dish == null)
            {
                logger.LogWarning("Блюдо с Id {DishId} не найдено", orderItem.DishId);
                return NotFound();
            }
            orderItem.Dish = dish;
            orderItem.OrderId = newOrderId;
            orderItem.Id = newItemId++;
        }

        entity.TotalAmount = entity.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        seeder.Orders.Add(entity);
        logger.LogInformation("Успешно создан заказ с Id {Id}", newOrderId);

        return CreatedAtAction(nameof(GetById), new { id = newOrderId }, mapper.Map<OrderDto>(entity));
    }

    /// <summary>
    /// Обновляет существующий заказ
    /// </summary>
    /// <param name="id">Идентификатор обновляемого заказа</param>
    /// <param name="dto">Модель данных для обновления</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] OrderCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var item = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка обновления несуществующего заказа с Id {Id}", id);
            return NotFound();
        }

        var client = seeder.Clients.FirstOrDefault(c => c.Id == dto.ClientId);
        var restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId);
        if (client == null || restaurant == null)
        {
            logger.LogWarning("Клиент или ресторан не найдены");
            return NotFound();
        }

        mapper.Map(dto, item);
        item.Client = client;
        item.Restaurant = restaurant;

        var newItemId = seeder.Orders
            .SelectMany(o => o.Items)
            .Any()
            ? seeder.Orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        foreach (var orderItem in item.Items)
        {
            var dish = seeder.Dishes.FirstOrDefault(d => d.Id == orderItem.DishId);
            if (dish == null)
            {
                logger.LogWarning("Блюдо с Id {DishId} не найдено", orderItem.DishId);
                return NotFound();
            }
            orderItem.Dish = dish;
            orderItem.OrderId = id;
            orderItem.Id = newItemId++;
        }

        item.TotalAmount = item.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        logger.LogInformation("Успешно обновлен заказ с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заказ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор удаляемого заказа</param>
    /// <returns>Статус 204 No Content или 404 Not Found</returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (item == null)
        {
            logger.LogWarning("Попытка удаления несуществующего заказа с Id {Id}", id);
            return NotFound();
        }

        seeder.Orders.Remove(item);
        logger.LogInformation("Успешно удален заказ с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает список позиций указанного заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <returns>Коллекция DTO позиций заказа</returns>
    [HttpGet("{id:int}/items")]
    public ActionResult<IEnumerable<OrderItemDto>> GetItems(int id)
    {
        var order = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            logger.LogWarning("Заказ с Id {Id} не найден", id);
            return NotFound();
        }

        return Ok(mapper.Map<IEnumerable<OrderItemDto>>(order.Items));
    }
}