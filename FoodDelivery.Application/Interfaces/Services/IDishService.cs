using FoodDelivery.Contracts.Dtos.Dishes;

namespace FoodDelivery.Application.Interfaces.Services;

/// <summary>
/// Определяет методы для управления блюдами
/// </summary>
public interface IDishService : IApplicationService<DishDto, DishCreateUpdateDto, int>;