using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO для создания или обновления ресторана
/// </summary>
public class RestaurantCreateDto
{
    /// <summary>
    /// Название ресторана
    /// </summary>
    [Required(ErrorMessage = "Название ресторана обязательно для заполнения")]
    public required string Name { get; set; }

    /// <summary>
    /// Физический адрес ресторана
    /// </summary>
    [Required(ErrorMessage = "Адрес ресторана обязателен для заполнения")]
    public required string Address { get; set; }

    /// <summary>
    /// Рейтинг ресторана (от 0 до 5)
    /// </summary>
    [Range(0, 5, ErrorMessage = "Рейтинг должен находиться в диапазоне от 0 до 5")]
    public double Rating { get; set; }

    /// <summary>
    /// Время открытия ресторана
    /// </summary>
    public TimeOnly OpeningTime { get; set; }

    /// <summary>
    /// Время закрытия ресторана
    /// </summary>
    public TimeOnly ClosingTime { get; set; }
}