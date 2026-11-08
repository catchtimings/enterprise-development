using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Категория блюда
/// </summary>
[Table("dish_categories")]
public class DishCategory
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Название категории
    /// </summary>
    [Column("name")]
    public required string Name { get; set; }
}