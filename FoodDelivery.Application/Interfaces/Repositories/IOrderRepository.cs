using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Определяет методы для управления заказами
/// </summary>
public interface IOrderRepository : IRepository<Order, int>;