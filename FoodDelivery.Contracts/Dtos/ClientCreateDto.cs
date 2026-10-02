using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO для создания или обновления клиента
/// </summary>
public class ClientCreateDto
{
    /// <summary>
    /// Полное имя клиента (ФИО)
    /// </summary>
    [Required(ErrorMessage = "ФИО клиента обязательно для заполнения")]
    public required string FullName { get; set; }

    /// <summary>
    /// Контактный номер телефона
    /// </summary>
    [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки по умолчанию
    /// </summary>
    [Required(ErrorMessage = "Адрес доставки обязателен для заполнения")]
    public required string DeliveryAddress { get; set; }
}