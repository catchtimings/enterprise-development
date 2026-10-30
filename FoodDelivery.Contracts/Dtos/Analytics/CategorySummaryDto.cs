namespace FoodDelivery.Contracts.Dtos.Analytics;

/// <summary>
/// Сводная информация по категориям блюд
/// </summary>
/// <param name="CategoryName">Название категории блюд</param>
/// <param name="OrderCount">Количество уникальных заказов с блюдами данной категории</param>
/// <param name="AverageSum">Средняя сумма чека по категории</param>
/// <param name="TotalSum">Общая сумма заказов по категории за период</param>
public record CategorySummaryDto(string CategoryName, int OrderCount, decimal AverageSum, decimal TotalSum);