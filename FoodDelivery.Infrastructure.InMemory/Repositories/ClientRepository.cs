using FoodDelivery.Application.Interfaces;
using FoodDelivery.Domain;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory.Repositories;

/// <summary>
/// Репозиторий клиентов в памяти
/// </summary>
public class ClientRepository(DataSeeder seeder) : IRepository<Client>
{
    /// <inheritdoc />
    public Task<IList<Client>> GetAll() => Task.FromResult<IList<Client>>(seeder.Clients);

    /// <inheritdoc />
    public Task<Client?> GetById(int id) => Task.FromResult(seeder.Clients.FirstOrDefault(c => c.Id == id));

    /// <inheritdoc />
    public Task<Client> Add(Client entity)
    {
        entity.Id = seeder.Clients.Any() ? seeder.Clients.Max(c => c.Id) + 1 : 1;
        seeder.Clients.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<Client> Update(Client entity)
    {
        var existing = seeder.Clients.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null)
        {
            existing.FullName = entity.FullName;
            existing.PhoneNumber = entity.PhoneNumber;
            existing.DeliveryAddress = entity.DeliveryAddress;
        }
        return Task.FromResult(existing!);
    }

    /// <inheritdoc />
    public Task<bool> Delete(int id)
    {
        var existing = seeder.Clients.FirstOrDefault(c => c.Id == id);
        if (existing == null) return Task.FromResult(false);

        seeder.Clients.Remove(existing);
        return Task.FromResult(true);
    }
}