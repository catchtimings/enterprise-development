using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Определяет методы для управления клиентами
/// </summary>
public interface IClientRepository : IRepository<Client, int>;