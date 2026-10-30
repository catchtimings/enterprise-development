namespace FoodDelivery.Contracts.Dtos.Clients;

/// <summary>
/// Клиент службы доставки
/// </summary>
/// <param name="Id">Уникальный идентификатор клиента</param>
/// <param name="FullName">Полное имя клиента (ФИО)</param>
/// <param name="PhoneNumber">Контактный номер телефона</param>
/// <param name="DeliveryAddress">Адрес доставки по умолчанию</param>
public record ClientDto(int Id, string FullName, string PhoneNumber, string DeliveryAddress);