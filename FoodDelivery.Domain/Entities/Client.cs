namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Клиент службы доставки
/// </summary>
public class Client
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// ФИО клиента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер телефона для связи
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки по умолчанию
    /// </summary>
    public required string DeliveryAddress { get; set; }
}