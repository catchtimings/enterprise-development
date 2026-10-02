namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO клиента с максимальной суммой трат
/// </summary>
public class TopSpenderDto
{
    /// <summary>
    /// Сведения о клиенте
    /// </summary>
    public required ClientDto Client { get; set; }

    /// <summary>
    /// Общая потраченная сумма за всё время
    /// </summary>
    public decimal TotalSpent { get; set; }
}