using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Dishes;

/// <summary>
/// DTO для создания или обновления блюда
/// </summary>
/// <param name="Name">Название блюда</param>
/// <param name="Weight">Вес блюда в граммах</param>
/// <param name="Price">Цена блюда в рублях</param>
/// <param name="CategoryId">Идентификатор категории блюда</param>
public record DishCreateDto(
    [property: Required(ErrorMessage = "Название блюда обязательно для заполнения")]
    string Name,
    [property: Range(1, 10000, ErrorMessage = "Вес должен быть в диапазоне от 1 до 10000 грамм")]
    int Weight,
    [property: Range(0, 1000000, ErrorMessage = "Цена должна быть неотрицательной")]
    decimal Price,
    [property: Required(ErrorMessage = "Идентификатор категории обязателен")]
    int CategoryId
);