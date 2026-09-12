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
    /// Рейтинг ресторана от 0.0 до 5.0
    /// </summary>
    public double Rating { get; set; }

    /// <summary>
    /// Часы работы
    /// </summary>
    public string? OpeningHours { get; set; }
}