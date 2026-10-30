using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Dishes;

/// <summary>
/// Данные для создания и обновления блюда
/// </summary>
/// <param name="Name">Название блюда</param>
/// <param name="Weight">Вес порции в граммах</param>
/// <param name="Price">Цена блюда</param>
/// <param name="CategoryId">Идентификатор категории</param>
public record DishCreateUpdateDto(
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [Range(1, 10000)] int Weight,
    [Range(typeof(decimal), "0.01", "1000000")] decimal Price,
    [Range(1, int.MaxValue)] int CategoryId
);