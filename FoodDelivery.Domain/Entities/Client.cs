namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Клиент службы доставки
/// </summary>
public class Client
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// ФИО клиента
    /// </summary>
    /// <example>Иванов Иван Иванович</example>
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