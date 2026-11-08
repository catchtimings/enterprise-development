using FoodDelivery.Contracts.Dtos.Clients;

namespace FoodDelivery.Application.Interfaces.Services;

/// <summary>
/// Определяет методы для управления клиентами
/// </summary>
public interface IClientService : IApplicationService<ClientDto, ClientCreateUpdateDto, int>;