using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Clients;

/// <summary>
/// DTO для создания или обновления клиента
/// </summary>
/// <param name="FullName">Полное имя клиента (ФИО)</param>
/// <param name="PhoneNumber">Контактный номер телефона</param>
/// <param name="DeliveryAddress">Адрес доставки по умолчанию</param>
public record ClientCreateDto(
    [property: Required(ErrorMessage = "ФИО клиента обязательно для заполнения")]
    string FullName,
    [property: Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    string PhoneNumber,
    [property: Required(ErrorMessage = "Адрес доставки обязателен для заполнения")]
    string DeliveryAddress
);