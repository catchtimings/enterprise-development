using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Ресторан службы доставки
/// </summary>
[Table("restaurants")]
public class Restaurant
{
    /// <summary>
    /// Уникальный идентификатор ресторана
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Название ресторана
    /// </summary>
    [Column("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Физический адрес ресторана
    /// </summary>
    [Column("address")]
    public required string Address { get; set; }

    /// <summary>
    /// Рейтинг ресторана
    /// </summary>
    [Column("rating")]
    public double Rating { get; set; }

    /// <summary>
    /// Время открытия ресторана
    /// </summary>
    [Column("opening_time")]
    public TimeOnly OpeningTime { get; set; }

    /// <summary>
    /// Время закрытия ресторана
    /// </summary>
    [Column("closing_time")]
    public TimeOnly ClosingTime { get; set; }
}