namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO ресторана службы доставки
/// </summary>
public class RestaurantDto
{
    /// <summary>
    /// Уникальный идентификатор ресторана
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название ресторана
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Физический адрес ресторана
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Рейтинг ресторана
    /// </summary>
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