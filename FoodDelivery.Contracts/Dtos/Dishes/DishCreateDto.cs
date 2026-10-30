namespace FoodDelivery.Contracts.Dtos.Dishes;

/// <summary>
/// DTO для создания или обновления блюда
/// </summary>
/// <param name="Name">Название блюда</param>
/// <param name="Weight">Вес блюда в граммах</param>
/// <param name="Price">Цена блюда в рублях</param>
/// <param name="CategoryId">Идентификатор категории блюда</param>
public record DishCreateDto(
    string Name,
    int Weight,
    decimal Price,
    int CategoryId
);