using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.DishCategories;

/// <summary>
/// Данные для создания и обновления категории блюд
/// </summary>
/// <param name="Name">Название категории</param>
public record DishCategoryCreateUpdateDto(
    [Required(ErrorMessage = "Название категории обязательно")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название должно быть от 1 до 100 символов")]
    string Name
);