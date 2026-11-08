using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.EFCore.Repositories;

/// <summary>
/// Репозиторий клиентов в базе данных
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class ClientEfCoreRepository(FoodDeliveryDbContext dbContext) : IClientRepository
{
    /// <inheritdoc />
    public async Task<Client> Add(Client entity)
    {
        var result = await dbContext.Clients.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(int id)
    {
        var entity = await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (entity == null)
            return false;

        dbContext.Clients.Remove(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<IList<Client>> GetAll() => await dbContext.Clients.ToListAsync();

    /// <inheritdoc />
    public async Task<Client?> GetById(int id) => await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);

    /// <inheritdoc />
    public async Task<Client> Update(Client entity)
    {
        var result = dbContext.Clients.Update(entity);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}

