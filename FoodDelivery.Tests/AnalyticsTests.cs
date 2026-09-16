using FoodDelivery.Domain;

namespace FoodDelivery.Tests;

public class AnalyticsTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DataSeeder _seeder = fixture.Seeder;

    [Fact]
    public void GetTop5RestaurantsByOrderCount_WhenOrdersExist_ReturnsTop5Restaurants()
    {
        // Arrange
        const int topCount = 5;
        var expectedRestaurantIds = new[] { 1, 2, 3, 4, 5 };

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

    [Fact]
    public void GetClientsByRestaurant_WhenRestaurantSelected_ReturnsClientsOrderedByFullName()
    {
        // Arrange
        const int selectedRestaurantId = 1;
        var expectedClientIds = new[] { 1 };
        var expectedFullNames = new[] { "Аннова Анна Ивановна" };

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

    [Fact]
    public void GetCategoryOrderSummary_ForGivenPeriod_ReturnsCorrectAggregations()
    {
        // Arrange
        var expectedCategoryCount = 10;

        var expectedPizzaOrderCount = 1;
        var expectedPizzaAverageSum = 150m;
        var expectedPizzaTotalSum = 150m;

        var expectedGrillOrderCount = 1;
        var expectedGrillAverageSum = 10500m;
        var expectedGrillTotalSum = 10500m;

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

    [Fact]
    public void GetTopSpenderClient_WhenOrdersExist_ReturnsClientWithMaxTotalAmount()
    {
        // Arrange
        const int expectedTopSpenderId = 10;
        const decimal expectedTotalSpent = 10500m;

        // Act
        var result = _seeder.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!)
            .Select(g => new
            {
                Client = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .ThenBy(x => x.Client.Id)
            .FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTopSpenderId, result.Client.Id);
        Assert.Equal(expectedTotalSpent, result.TotalSpent);
    }
}