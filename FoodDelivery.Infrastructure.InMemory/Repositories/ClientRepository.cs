using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий клиентов в памяти
/// </summary>
public class ClientRepository : IClientRepository
{
    /// <inheritdoc />
    public Task<IList<Client>> GetAll() => Task.FromResult<IList<Client>>(DataSeeder.Clients);

    /// <inheritdoc />
    public Task<Client?> GetById(int id) => Task.FromResult(DataSeeder.Clients.FirstOrDefault(c => c.Id == id));

    /// <inheritdoc />
    public Task<Client> Add(Client entity)
    {
        entity.Id = DataSeeder.Clients.Count > 0 ? DataSeeder.Clients.Max(c => c.Id) + 1 : 1;
        DataSeeder.Clients.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Client> Update(Client entity)
    {
        var existing = DataSeeder.Clients.First(c => c.Id == entity.Id);

        existing.FullName = entity.FullName;
        existing.PhoneNumber = entity.PhoneNumber;
        existing.DeliveryAddress = entity.DeliveryAddress;
        return Task.FromResult(existing);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = DataSeeder.Clients.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult(false);

        DataSeeder.Clients.Remove(existing);
        return Task.FromResult(true);
    }
}