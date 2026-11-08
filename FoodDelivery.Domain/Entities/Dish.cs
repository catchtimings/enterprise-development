using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Блюдо ресторана
/// </summary>
[Table("dishes")]
public class Dish
{
    /// <summary>
    /// Идентификатор блюда
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Название блюда
    /// </summary>
    [Column("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Вес блюда в граммах
    /// </summary>
    [Column("weight")]
    public int Weight { get; set; }

    /// <summary>
    /// Цена блюда в рублях
    /// </summary>
    [Column("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Идентификатор категории блюда
    /// </summary>
    [Column("category_id")]
    public required int CategoryId { get; set; }

    /// <summary>
    /// Категория блюда
    /// </summary>
    public DishCategory? Category { get; set; }
}