namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Ресторан службы доставки
/// </summary>
public class Restaurant
{
    /// <summary>
    /// Уникальный идентификатор ресторана
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

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
    public TimeOnly OpeningHours { get; set; }

    /// <summary>
    /// Время закрытия ресторана
    /// </summary>
    public TimeOnly ClosingHours { get; set; }
}