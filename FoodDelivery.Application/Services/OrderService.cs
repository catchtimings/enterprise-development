using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Orders;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления заказами
/// </summary>
public class OrderService(
    IRepository<Order> orderRepository,
    IRepository<Client> clientRepository,
    IRepository<Restaurant> restaurantRepository,
    IRepository<Dish> dishRepository)
{
    /// <summary>
    /// Создает новый заказ
    /// </summary>
    public async Task<OrderDto> Create(OrderCreateUpdateDto dto)
    {
        var client = await clientRepository.GetById(dto.ClientId)
            ?? throw new KeyNotFoundException("Клиент не найден");
        var restaurant = await restaurantRepository.GetById(dto.RestaurantId)
            ?? throw new KeyNotFoundException("Ресторан не найден");

        DateTime createdAtVar = DateTime.UtcNow;

        if (dto.DeliveredAt.HasValue && dto.DeliveredAt.Value < createdAtVar)
            throw new ArgumentException("Время доставки не может быть раньше времени создания заказа");


        var items = new List<OrderItem>();
        foreach (var itemDto in dto.Items)
        {
            var dish = await dishRepository.GetById(itemDto.DishId)
                ?? throw new KeyNotFoundException($"Блюдо {itemDto.DishId} не найдено");

            var item = OrderMapper.Map(itemDto);
            item.Dish = dish;
            item.PriceAtOrder = dish.Price;
            items.Add(item);
        }

        var order = new Order
        {
            Id = 0,
            ClientId = client.Id,
            Client = client,
            RestaurantId = restaurant.Id,
            Restaurant = restaurant,
            CreatedAt = createdAtVar,
            DeliveredAt = dto.DeliveredAt,
            Items = items
        };
        order.TotalAmount = order.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        await orderRepository.Add(order);
        return OrderMapper.Map(order);
    }

    /// <summary>
    /// Возвращает заказ по идентификатору
    /// </summary>
    public async Task<OrderDto?> Get(int id)
    {
        var order = await orderRepository.GetById(id);
        return order is null ? null : OrderMapper.Map(order);
    }

    /// <summary>
    /// Возвращает список всех заказов
    /// </summary>
    public async Task<IList<OrderDto>> GetAll()
    {
        var orders = await orderRepository.GetAll();
        return OrderMapper.Map(orders.ToList());
    }

    /// <summary>
    /// Обновляет данные заказа
    /// </summary>
    public async Task<OrderDto> Update(int id, OrderCreateUpdateDto dto)
    {
        var existing = await orderRepository.GetById(id)
            ?? throw new KeyNotFoundException("Заказ не найден");
        var client = await clientRepository.GetById(dto.ClientId)
            ?? throw new KeyNotFoundException("Клиент не найден");
        var restaurant = await restaurantRepository.GetById(dto.RestaurantId)
            ?? throw new KeyNotFoundException("Ресторан не найден");

        if (dto.DeliveredAt.HasValue)
        {
            if (dto.DeliveredAt.Value < existing.CreatedAt)
                throw new ArgumentException("Время доставки не может быть раньше времени создания заказа");

            existing.DeliveredAt = dto.DeliveredAt.Value;
        }

        var items = new List<OrderItem>();
        foreach (var itemDto in dto.Items)
        {
            var dish = await dishRepository.GetById(itemDto.DishId)
                ?? throw new KeyNotFoundException($"Блюдо {itemDto.DishId} не найдено");

            var item = OrderMapper.Map(itemDto);
            item.OrderId = id;
            item.Dish = dish;
            item.PriceAtOrder = dish.Price;
            items.Add(item);
        }

        existing.ClientId = client.Id;
        existing.Client = client;
        existing.RestaurantId = restaurant.Id;
        existing.Restaurant = restaurant;
        existing.DeliveredAt = dto.DeliveredAt;
        existing.Items = items;
        existing.TotalAmount = existing.Items.Sum(i => i.Quantity * i.PriceAtOrder);

        await orderRepository.Update(existing);
        return OrderMapper.Map(existing);
    }

    /// <summary>
    /// Удаляет заказ
    /// </summary>
    public Task Delete(int id) => orderRepository.Delete(id);

    /// <summary>
    /// Возвращает позиции указанного заказа
    /// </summary>
    public async Task<IList<OrderItemDto>> GetItems(int orderId)
    {
        var order = await orderRepository.GetById(orderId)
            ?? throw new KeyNotFoundException("Заказ не найден");

        return order.Items.Select(OrderMapper.Map).ToList();
    }
}