using FoodDelivery.Contracts.Dtos.Clients;

namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// Клиент с максимальной суммой трат
/// </summary>
/// <param name="Client">Сведения о клиенте</param>
/// <param name="TotalSpent">Общая потраченная сумма за всё время</param>
public record TopSpenderDto(ClientDto Client, decimal TotalSpent);