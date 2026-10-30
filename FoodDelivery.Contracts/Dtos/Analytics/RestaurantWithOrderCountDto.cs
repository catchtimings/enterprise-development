using FoodDelivery.Contracts.Dtos.Restaurants;

namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// Ресторан с количеством его заказов
/// </summary>
/// <param name="Restaurant">Сведения о ресторане</param>
/// <param name="OrderCount">Количество заказов ресторана</param>
public record RestaurantWithOrderCountDto(RestaurantDto Restaurant, int OrderCount);