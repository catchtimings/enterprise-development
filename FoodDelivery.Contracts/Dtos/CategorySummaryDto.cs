namespace FoodDelivery.Contracts.Dtos;

/// <summary>
/// DTO сводной информации по категориям блюд
/// </summary>
public class CategorySummaryDto
{
    /// <summary>
    /// Название категории блюд
    /// </summary>
    public required string CategoryName { get; set; }

    /// <summary>
    /// Количество уникальных заказов с блюдами данной категории
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// Средняя сумма чека по категории
    /// </summary>
    public decimal AverageSum { get; set; }

    /// <summary>
    /// Общая сумма заказов по категории за период
    /// </summary>
    public decimal TotalSum { get; set; }
}