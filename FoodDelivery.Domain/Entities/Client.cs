using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Клиент службы доставки
/// </summary>
[Table("clients")]
public class Client
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// ФИО клиента
    /// </summary>
    [Column("full_name")]
    public required string FullName { get; set; }

    /// <summary>
    /// Номер телефона для связи
    /// </summary>
    [Column("phone_number")]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки по умолчанию
    /// </summary>
    [Column("delivery_address")]
    public required string DeliveryAddress { get; set; }
}