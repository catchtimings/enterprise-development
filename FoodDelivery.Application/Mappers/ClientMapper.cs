using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Mappers;

/// <summary>
/// Преобразует сущности Client в DTO и обратно
/// </summary>
public static class ClientMapper
{
    public static ClientDto Map(Client entity)
        => new(entity.Id, entity.FullName, entity.PhoneNumber, entity.DeliveryAddress);

    public static Client Map(ClientCreateUpdateDto dto) => new()
    {
        Id = 0,
        FullName = dto.FullName,
        PhoneNumber = dto.PhoneNumber,
        DeliveryAddress = dto.DeliveryAddress
    };

    public static List<ClientDto> Map(List<Client> entities) => [.. entities.Select(Map)];
}