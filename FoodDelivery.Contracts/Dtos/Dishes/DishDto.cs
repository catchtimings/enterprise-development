namespace FoodDelivery.Contracts.Dtos.Dishes;

/// <summary>
/// DTO блюда ресторана
/// </summary>
/// <param name="Id">Идентификатор блюда</param>
/// <param name="Name">Название блюда</param>
/// <param name="Weight">Вес блюда в граммах</param>
/// <param name="Price">Цена блюда в рублях</param>
/// <param name="CategoryId">Идентификатор категории блюда</param>
/// <param name="CategoryName">Название категории блюда</param>
public record DishDto(int Id, string Name, int Weight, decimal Price, int CategoryId, string? CategoryName);