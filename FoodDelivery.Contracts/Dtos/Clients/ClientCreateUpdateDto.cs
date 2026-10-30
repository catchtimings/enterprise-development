using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Contracts.Dtos.Clients;

/// <summary>
/// Данные для создания и обновления клиента
/// </summary>
/// <param name="FullName">ФИО клиента</param>
/// <param name="PhoneNumber">Номер телефона</param>
/// <param name="DeliveryAddress">Адрес доставки</param>
public record ClientCreateUpdateDto(
    [Required, StringLength(200, MinimumLength = 1)] string FullName,
    [Required, StringLength(30, MinimumLength = 1)] string PhoneNumber,
    [Required, StringLength(300, MinimumLength = 1)] string DeliveryAddress
);