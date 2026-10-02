namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO клиента службы доставки
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Уникальный идентификатор клиента
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя клиента (ФИО)
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Контактный номер телефона
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки по умолчанию
    /// </summary>
    public required string DeliveryAddress { get; set; }
}