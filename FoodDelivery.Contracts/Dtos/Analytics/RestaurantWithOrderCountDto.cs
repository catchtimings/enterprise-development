using FoodDelivery.Contracts.Dtos.Restaurants;

namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// DTO ресторана с количеством его заказов
/// </summary>
public class RestaurantWithOrderCountDto
{
    /// <summary>
    /// Сведения о ресторане
    /// </summary>
    public required RestaurantDto Restaurant { get; set; }

    /// <summary>
    /// Количество заказов ресторана
    /// </summary>
    public int OrderCount { get; set; }
}