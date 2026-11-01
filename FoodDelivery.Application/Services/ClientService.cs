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
    public async Task<ClientDto> Create(ClientCreateUpdateDto dto)
    {
        var client = ClientMapper.Map(dto);
        var createdClient = await clientRepository.Add(client);
        return ClientMapper.Map(createdClient);
    }

    /// <summary>
    /// Возвращает клиента по идентификатору
    /// </summary>
    public async Task<ClientDto?> Get(int id)
    {
        var client = await clientRepository.GetById(id);
        return client is null ? null : ClientMapper.Map(client);
    }

    /// <summary>
    /// Возвращает всех клиентов
    /// </summary>
    public async Task<IList<ClientDto>> GetAll()
    {
        var clients = await clientRepository.GetAll();
        return ClientMapper.Map(clients.ToList());
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    public async Task<ClientDto> Update(int id, ClientCreateUpdateDto dto)
    {
        var existing = await clientRepository.GetById(id)
            ?? throw new KeyNotFoundException("Клиент не найден");

        existing.FullName = dto.FullName;
        existing.PhoneNumber = dto.PhoneNumber;
        existing.DeliveryAddress = dto.DeliveryAddress;

        var updatedClient = await clientRepository.Update(existing);
        return ClientMapper.Map(updatedClient);
    }

    /// <summary>
    /// Удаляет клиента
    /// </summary>
    public Task<bool> Delete(int id) => clientRepository.Delete(id);
}