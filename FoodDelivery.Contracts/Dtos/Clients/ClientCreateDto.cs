namespace FoodDelivery.Contracts.Dtos.Clients;

/// <summary>
/// DTO для создания или обновления клиента
/// </summary>
/// <param name="FullName">Полное имя клиента (ФИО)</param>
/// <param name="PhoneNumber">Контактный номер телефона</param>
/// <param name="DeliveryAddress">Адрес доставки по умолчанию</param>
public record ClientCreateDto(
    string FullName,
    string PhoneNumber,
    string DeliveryAddress
);