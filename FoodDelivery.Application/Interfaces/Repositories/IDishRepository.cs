using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Определяет методы для управления блюдами
/// </summary>
public interface IDishRepository : IRepository<Dish, int>;