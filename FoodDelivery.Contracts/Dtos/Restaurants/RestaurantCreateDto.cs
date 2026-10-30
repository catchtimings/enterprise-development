using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Restaurants;

/// <summary>
/// DTO для создания или обновления ресторана
/// </summary>
/// <param name="Name">Название ресторана</param>
/// <param name="Address">Физический адрес ресторана</param>
/// <param name="Rating">Рейтинг ресторана</param>
/// <param name="OpeningTime">Время открытия ресторана</param>
/// <param name="ClosingTime">Время закрытия ресторана</param>
public record RestaurantCreateDto(
    [property: Required(ErrorMessage = "Название ресторана обязательно для заполнения")]
    string Name,
    [property: Required(ErrorMessage = "Адрес ресторана обязателен для заполнения")]
    string Address,
    [property: Range(0, 5, ErrorMessage = "Рейтинг должен находиться в диапазоне от 0 до 5")]
    double Rating,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime
);