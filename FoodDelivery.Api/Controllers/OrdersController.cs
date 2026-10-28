using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер для управления заказами
/// </summary>
/// <param name="seeder">Хранилище тестовых данных</param>
/// <param name="logger">Логгер событий</param>
[ApiController]
[Route("api/orders")]
public class OrdersController(DataSeeder seeder, ILogger<OrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает список всех заказов
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAll()
    {
        logger.LogInformation("Получение полного списка заказов");
        return Ok(seeder.Orders.Select(MapToDto).ToList());
    }

    /// <summary>
    /// Возвращает заказ по его уникальному идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<OrderDto> GetById(int id)
    {
        var order = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            logger.LogWarning("Заказ с Id {Id} не найден", id);
            return NotFound();
        }
        return Ok(MapToDto(order));
    }

    /// <summary>
    /// Создает новый заказ.
    /// Дата оформления и итоговая сумма назначаются на стороне сервера.
    /// </summary>
    [HttpPost]
    public ActionResult<OrderDto> Create([FromBody] OrderCreateDto dto)
    {
        var client = seeder.Clients.FirstOrDefault(c => c.Id == dto.ClientId);
        var restaurant = seeder.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId);
        if (client == null || restaurant == null)
        {
            logger.LogWarning("Клиент или ресторан не найдены");
            return NotFound();
        }

        var newOrderId = seeder.Orders.Any() ? seeder.Orders.Max(o => o.Id) + 1 : 1;
        var newItemId = seeder.Orders.SelectMany(o => o.Items).Any()
            ? seeder.Orders.SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        var items = new List<OrderItem>();
        foreach (var itemDto in dto.Items)
        {
            var dish = seeder.Dishes.FirstOrDefault(d => d.Id == itemDto.DishId);
            if (dish == null)
            {
                logger.LogWarning("Блюдо с Id {DishId} не найдено", itemDto.DishId);
                return NotFound();
            }

            items.Add(new OrderItem
            {
                Id = newItemId++,
                OrderId = newOrderId,
                DishId = dish.Id,
                Dish = dish,
                Quantity = itemDto.Quantity,
                PriceAtOrder = dish.Price
            });
        }

        var entity = new Order
        {
            Id = newOrderId,
            ClientId = client.Id,
            Client = client,
            RestaurantId = restaurant.Id,
            Restaurant = restaurant,
            CreatedAt = DateTime.UtcNow,
            DeliveredAt = dto.DeliveredAt,
            Items = items
        };
        entity.TotalAmount = entity.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        seeder.Orders.Add(entity);
        logger.LogInformation("Успешно создан заказ с Id {Id}", newOrderId);

        return CreatedAtAction(nameof(GetById), new { id = newOrderId }, MapToDto(entity));
    }

    /// <summary>
    /// Обновляет существующий заказ
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] OrderCreateDto dto)
    {
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

        // сначала валидируем все блюда, чтобы не получить частично обновлённый заказ
        var dishes = new List<Dish>();
        foreach (var itemDto in dto.Items)
        {
            var dish = seeder.Dishes.FirstOrDefault(d => d.Id == itemDto.DishId);
            if (dish == null)
            {
                logger.LogWarning("Блюдо с Id {DishId} не найдено", itemDto.DishId);
                return NotFound();
            }
            dishes.Add(dish);
        }

        var newItemId = seeder.Orders
            .Where(o => o.Id != id)
            .SelectMany(o => o.Items)
            .Any()
            ? seeder.Orders.Where(o => o.Id != id).SelectMany(o => o.Items).Max(i => i.Id) + 1
            : 1;

        item.ClientId = client.Id;
        item.Client = client;
        item.RestaurantId = restaurant.Id;
        item.Restaurant = restaurant;
        item.DeliveredAt = dto.DeliveredAt;

        item.Items = dto.Items
            .Select((itemDto, index) => new OrderItem
            {
                Id = newItemId++,
                OrderId = id,
                DishId = dishes[index].Id,
                Dish = dishes[index],
                Quantity = itemDto.Quantity,
                PriceAtOrder = dishes[index].Price
            })
            .ToList();

        item.TotalAmount = item.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        logger.LogInformation("Успешно обновлен заказ с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет заказ по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var item = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (item != null) seeder.Orders.Remove(item);
        logger.LogInformation("Успешно удален заказ с Id {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Возвращает список позиций указанного заказа
    /// </summary>
    [HttpGet("{id:int}/items")]
    public ActionResult<IEnumerable<OrderItemDto>> GetItems(int id)
    {
        var order = seeder.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            logger.LogWarning("Заказ с Id {Id} не найден", id);
            return NotFound();
        }

        return Ok(order.Items.Select(MapItemToDto).ToList());
    }

    private static OrderDto MapToDto(Order order) => new()
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
        Items = order.Items.Select(MapItemToDto).ToList()
    };

    private static OrderItemDto MapItemToDto(OrderItem item) => new()
    {
        Id = item.Id,
        OrderId = item.OrderId,
        DishId = item.DishId,
        DishName = item.Dish?.Name,
        Quantity = item.Quantity,
        PriceAtOrder = item.PriceAtOrder
    };
}