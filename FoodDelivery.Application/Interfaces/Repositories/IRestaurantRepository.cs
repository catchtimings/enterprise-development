using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Определяет методы для управления рестроранами
/// </summary>
public interface IRestaurantRepository : IRepository<Restaurant, int>;