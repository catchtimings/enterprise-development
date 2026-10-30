using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Clients;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления клиентами
/// </summary>
public class ClientService(IRepository<Client> clientRepository)
{
    /// <summary>
    /// Создает нового клиента
    /// </summary>
    public async Task<ClientDto> Create(ClientCreateDto dto)
    {
        var client = ClientMapper.ToEntity(dto);
        await clientRepository.Add(client);
        return ClientMapper.ToDto(client);
    }

    /// <summary>
    /// Возвращает клиента по идентификатору
    /// </summary>
    public async Task<ClientDto?> Get(int id)
    {
        var client = await clientRepository.GetById(id);
        return client is null ? null : ClientMapper.ToDto(client);
    }

    /// <summary>
    /// Возвращает всех клиентов
    /// </summary>
    public async Task<IEnumerable<ClientDto>> GetAll()
    {
        var clients = await clientRepository.GetAll();
        return clients.Select(ClientMapper.ToDto);
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    public async Task<ClientDto> Update(int id, ClientCreateDto dto)
    {
        var existing = await clientRepository.GetById(id)
            ?? throw new KeyNotFoundException("Клиент не найден");

        existing.FullName = dto.FullName;
        existing.PhoneNumber = dto.PhoneNumber;
        existing.DeliveryAddress = dto.DeliveryAddress;

        await clientRepository.Update(existing);
        return ClientMapper.ToDto(existing);
    }

    /// <summary>
    /// Удаляет клиента
    /// </summary>
    public Task Delete(int id) => clientRepository.Delete(id);
}