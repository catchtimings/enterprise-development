using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Restaurants;

/// <summary>
/// Данные для создания и обновления ресторана
/// </summary>
/// <param name="Name">Название ресторана</param>
/// <param name="Address">Адрес ресторана</param>
/// <param name="Rating">Рейтинг от 0 до 5</param>
/// <param name="OpeningTime">Время открытия</param>
/// <param name="ClosingTime">Время закрытия</param>
public record RestaurantCreateUpdateDto(
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [Required, StringLength(300, MinimumLength = 1)] string Address,
    [Range(0.0, 5.0)] double Rating,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime
);