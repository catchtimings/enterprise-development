using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Определяет методы для управления категориями блюд
/// </summary>
public interface IDishCategoryRepository : IRepository<DishCategory, int>;