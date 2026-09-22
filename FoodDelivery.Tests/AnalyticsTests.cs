using FoodDelivery.Domain;

namespace FoodDelivery.Tests;

public class AnalyticsTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DataSeeder _seeder = fixture.Seeder;

    /// <summary>
    /// Проверяет получение пяти ресторанов с наибольшим количеством заказов
    /// </summary>
    [Fact]
    public void GetTop5RestaurantsByOrderCount_WhenOrdersExist_ReturnsTop5Restaurants()
    {
        // Arrange
        const int topCount = 5;
        var expectedRestaurantIds = new[] { 10, 1, 2, 3, 4 };

        // Act
        var result = _seeder.Orders
            .GroupBy(o => o.RestaurantId)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(topCount)
            .Select(g => g.Key)
            .ToArray();

        // Assert
        Assert.Equal(expectedRestaurantIds, result);
    }

    /// <summary>
    /// Проверяет получение заказов с минимальным временем доставки
    /// </summary>
    [Fact]
    public void GetOrdersWithMinDeliveryTime_WhenOrdersExist_ReturnsFastestOrders()
    {
        // Arrange
        var expectedMinDuration = TimeSpan.FromMinutes(15);
        var expectedOrderIds = new[] { 1, 2 };

        // Act
        var minDeliveryDuration = _seeder.Orders
            .Min(o => o.DeliveryDuration);

        var result = _seeder.Orders
            .Where(o => o.DeliveryDuration == minDeliveryDuration)
            .Select(o => o.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedMinDuration, minDeliveryDuration);
        Assert.Equal(expectedOrderIds, result);
    }

    /// <summary>
    /// Проверяет получение клиентов выбранного ресторана, отсортированных по ФИО
    /// </summary>
    [Fact]
    public void GetClientsByRestaurant_WhenRestaurantSelected_ReturnsClientsOrderedByFullName()
    {
        // Arrange
        const int selectedRestaurantId = 10;
        var expectedClientIds = new[] { 10, 11 };
        var expectedFullNames = new[] { "Сидоров Сидор Сидорович", "Смирнов Станислав Сергеевич" };

        // Act
        var result = _seeder.Orders
            .Where(o =>
                o.RestaurantId == selectedRestaurantId &&
                o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToArray();

        // Assert
        Assert.Equal(expectedClientIds, result.Select(c => c.Id).ToArray());
        Assert.Equal(expectedFullNames, result.Select(c => c.FullName).ToArray());
    }

    /// <summary>
    /// Проверяет получение агрегированной статистики по категориям за заданный период
    /// </summary>
    [Fact]
    public void GetCategoryOrderSummary_ForGivenPeriod_ReturnsCorrectAggregations()
    {
        // Arrange
        var expectedCategoryCount = 10;

        var expectedPizzaOrderCount = 1;
        var expectedPizzaAverageSum = 150m;
        var expectedPizzaTotalSum = 150m;

        var expectedGrillOrderCount = 2;
        var expectedGrillAverageSum = 10500m;
        var expectedGrillTotalSum = 21000m;

        var startDate = new DateTime(2026, 9, 10, 0, 0, 0);
        var endDate = new DateTime(2026, 9, 12, 23, 59, 59);

        // Act
        var result = _seeder.Orders
            .Where(o =>
                o.CreatedAt >= startDate &&
                o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .GroupBy(item => item.Dish!.Category!)
            .Select(g => new
            {
                CategoryName = g.Key.Name,
                OrderCount = g.Select(item => item.OrderId)
                    .Distinct()
                    .Count(),
                AverageSum = g.Average(item =>
                    item.Quantity * item.PriceAtOrder),
                TotalSum = g.Sum(item =>
                    item.Quantity * item.PriceAtOrder)
            })
            .OrderBy(s => s.CategoryName)
            .ToList();

        // Assert
        Assert.Equal(expectedCategoryCount, result.Count);

        var pizzaSummary = result
            .Single(s => s.CategoryName == "Пицца");

        Assert.Equal(expectedPizzaOrderCount, pizzaSummary.OrderCount);
        Assert.Equal(expectedPizzaAverageSum, pizzaSummary.AverageSum);
        Assert.Equal(expectedPizzaTotalSum, pizzaSummary.TotalSum);

        var grillSummary = result
            .Single(s => s.CategoryName == "Гриль");

        Assert.Equal(expectedGrillOrderCount, grillSummary.OrderCount);
        Assert.Equal(expectedGrillAverageSum, grillSummary.AverageSum);
        Assert.Equal(expectedGrillTotalSum, grillSummary.TotalSum);
    }

    /// <summary>
    /// Проверяет получение всех клиентов с максимальной общей суммой заказов
    /// </summary>
    [Fact]
    public void GetTopSpenderClients_WhenOrdersExist_ReturnsClientsWithMaxTotalAmount()
    {
        // Arrange
        var expectedTopSpenderIds = new[] { 10, 11 };
        const decimal expectedTotalSpent = 10500m;

        // Act
        var clientSpending = _seeder.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!)
            .Select(g => new
            {
                Client = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .ToList();

        var maxTotalSpent = clientSpending.Max(x => x.TotalSpent);

        var result = clientSpending
            .Where(x => x.TotalSpent == maxTotalSpent)
            .OrderBy(x => x.Client.Id)
            .ToArray();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(expectedTopSpenderIds, result.Select(x => x.Client.Id).ToArray());
        Assert.All(result, x => Assert.Equal(expectedTotalSpent, x.TotalSpent));
    }
}