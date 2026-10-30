namespace FoodDelivery.Contracts.Dtos.Restaurants;

/// <summary>
/// DTO ресторана службы доставки
/// </summary>
/// <param name="Id">Уникальный идентификатор ресторана</param>
/// <param name="Name">Название ресторана</param>
/// <param name="Address">Физический адрес ресторана</param>
/// <param name="Rating">Рейтинг ресторана</param>
/// <param name="OpeningTime">Время открытия ресторана</param>
/// <param name="ClosingTime">Время закрытия ресторана</param>
public record RestaurantDto(int Id, string Name, string Address, double Rating, TimeOnly OpeningTime, TimeOnly ClosingTime);