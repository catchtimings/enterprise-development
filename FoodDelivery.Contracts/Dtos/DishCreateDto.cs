using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO для создания или обновления блюда
/// </summary>
public class DishCreateDto
{
    /// <summary>
    /// Название блюда
    /// </summary>
    [Required(ErrorMessage = "Название блюда обязательно для заполнения")]
    public required string Name { get; set; }

    /// <summary>
    /// Вес блюда в граммах
    /// </summary>
    [Range(1, 10000, ErrorMessage = "Вес должен быть в диапазоне от 1 до 10000 грамм")]
    public int Weight { get; set; }

    /// <summary>
    /// Цена блюда в рублях
    /// </summary>
    [Range(0, 1000000, ErrorMessage = "Цена должна быть неотрицательной")]
    public decimal Price { get; set; }

    /// <summary>
    /// Идентификатор категории блюда
    /// </summary>
    [Required(ErrorMessage = "Идентификатор категории обязателен")]
    public required int CategoryId { get; set; }
}