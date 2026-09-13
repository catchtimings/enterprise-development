using FoodDelivery.Domain;

namespace FoodDelivery.Tests;

public class AnalyticsTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DataSeeder _seeder = fixture.Seeder;

    [Fact]
    public void GetTop5RestaurantsByOrderCount_WhenOrdersExist_ReturnsTop5Restaurants()
    {
        // Arrange
        const int expectedMaxCount = 5;

        // Act
        var result = _seeder.Orders
            .GroupBy(o => o.Restaurant)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key!.Name)
            .Take(expectedMaxCount)
            .Select(g => g.Key)
            .ToList();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count <= expectedMaxCount);
    }

    [Fact]
    public void GetOrdersWithMinDeliveryTime_WhenOrdersExist_ReturnsFastestOrders()
    {
        // Arrange
        var expectedMinDuration = TimeSpan.FromMinutes(15);
        const int expectedOrdersCount = 2;

        // Act
        var minDeliveryTime = _seeder.Orders.Min(o => o.DeliveryDuration);
        var result = _seeder.Orders
            .Where(o => o.DeliveryDuration == minDeliveryTime)
            .ToList();

        // Assert
        Assert.Equal(expectedMinDuration, minDeliveryTime);
        Assert.Equal(expectedOrdersCount, result.Count);
        Assert.All(result, o => Assert.Equal(expectedMinDuration, o.DeliveryDuration));
    }

    [Fact]
    public void GetClientsByRestaurant_WhenRestaurantSelected_ReturnsClientsOrderedByFullName()
    {
        // Arrange
        var targetRestaurantId = _seeder.Restaurants.First().Id;

        // Act
        var result = _seeder.Orders
            .Where(o => o.RestaurantId == targetRestaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        // Assert
        Assert.NotNull(result);
        var expectedSortedNames = result.Select(c => c.FullName).OrderBy(n => n).ToList();
        var actualNames = result.Select(c => c.FullName).ToList();
        Assert.Equal(expectedSortedNames, actualNames);
    }

    [Fact]
    public void GetCategoryOrderSummary_ForGivenPeriod_ReturnsCorrectAggregations()
    {
        // Arrange
        var startDate = DateTimeOffset.UtcNow.AddDays(-10);
        var endDate = DateTimeOffset.UtcNow;
        const int expectedCategoriesCount = 10;

        // Act
        var result = _seeder.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .GroupBy(item => item.Dish!.Category!)
            .Select(g => new
            {
                CategoryName = g.Key.Name,
                OrderCount = g.Count(),
                AverageSum = g.Average(item => item.Quantity * item.PriceAtOrder),
                TotalSum = g.Sum(item => item.Quantity * item.PriceAtOrder)
            })
            .ToList();

        // Assert
        Assert.Equal(expectedCategoriesCount, result.Count);
        Assert.All(result, summary =>
        {
            Assert.True(summary.OrderCount > 0);
            Assert.True(summary.TotalSum > 0);
        });
    }

    [Fact]
    public void GetTopSpenderClient_WhenOrdersExist_ReturnsClientWithMaxTotalAmount()
    {
        // Arrange
        var expectedTopSpenderId = _seeder.Clients.Last().Id;

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
            .FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTopSpenderId, result.Client.Id);
    }
}