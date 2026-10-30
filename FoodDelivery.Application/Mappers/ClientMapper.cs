using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущность Client в DTO и обратно
/// </summary>
public static class ClientMapper
{
    /// <summary>
    /// Преобразует сущность в DTO
    /// </summary>
    public static ClientDto ToDto(Client entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        PhoneNumber = entity.PhoneNumber,
        DeliveryAddress = entity.DeliveryAddress
    };

    /// <summary>
    /// Создает сущность из DTO
    /// </summary>
    public static Client ToEntity(ClientCreateDto dto) => new()
    {
        Id = 0,
        FullName = dto.FullName,
        PhoneNumber = dto.PhoneNumber,
        DeliveryAddress = dto.DeliveryAddress
    };
}