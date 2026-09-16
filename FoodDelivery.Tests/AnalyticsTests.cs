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

        var expectedRestaurantIds = _seeder.Orders
            .GroupBy(o => o.RestaurantId)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(topCount)
            .Select(g => g.Key)
            .ToList();

        // Act
        var result = _seeder.Orders
            .GroupBy(o => o.RestaurantId)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(topCount)
            .Select(g => g.Key)
            .ToList();

        // Assert
        Assert.Equal(topCount, result.Count);
        Assert.Equal(expectedRestaurantIds, result);
    }

    [Fact]
    public void GetOrdersWithMinDeliveryTime_WhenOrdersExist_ReturnsFastestOrders()
    {
        // Arrange
        var minDeliveryDuration = _seeder.Orders.Min(o => o.DeliveryDuration);

        var expectedOrderIds = _seeder.Orders
            .Where(o => o.DeliveryDuration == minDeliveryDuration)
            .Select(o => o.Id)
            .ToList();

        // Act
        var result = _seeder.Orders
            .Where(o => o.DeliveryDuration == minDeliveryDuration)
            .ToList();

        // Assert
        Assert.Equal(expectedOrderIds.Count, result.Count);
        Assert.Equal(expectedOrderIds, result.Select(o => o.Id));
        Assert.All(result, order =>
            Assert.Equal(minDeliveryDuration, order.DeliveryDuration));
    }

    [Fact]
    public void GetClientsByRestaurant_WhenRestaurantSelected_ReturnsClientsOrderedByFullName()
    {
        // Arrange
        var restaurantId = _seeder.Restaurants.First().Id;

        var expectedClientIds = _seeder.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!.Id)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

        // Act
        var result = _seeder.Orders
            .Where(o => o.RestaurantId == restaurantId && o.Client != null)
            .Select(o => o.Client!)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Select(c => c.Id).Distinct().Count(), result.Count);
        Assert.Equal(
            result.OrderBy(c => c.FullName).Select(c => c.Id),
            result.Select(c => c.Id));
    }

    [Fact]
    public void GetCategoryOrderSummary_ForGivenPeriod_ReturnsCorrectAggregations()
    {
        // Arrange
        var startDate = DateTimeOffset.UtcNow.AddDays(-10);
        var endDate = DateTimeOffset.UtcNow;

        var expectedCategories = _seeder.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .Select(item => item.Dish!.Category!.Name)
            .Distinct()
            .ToList();

        // Act
        var result = _seeder.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .SelectMany(o => o.Items)
            .Where(item => item.Dish?.Category != null)
            .GroupBy(item => item.Dish!.Category!)
            .Select(g => new
            {
                CategoryName = g.Key.Name,
                OrderCount = g.Select(item => item.OrderId).Distinct().Count(),
                AverageSum = g.Average(item => item.Quantity * item.PriceAtOrder),
                TotalSum = g.Sum(item => item.Quantity * item.PriceAtOrder)
            })
            .ToList();

        // Assert
        Assert.Equal(expectedCategories.Count, result.Count);
        Assert.All(result, summary =>
        {
            Assert.False(string.IsNullOrWhiteSpace(summary.CategoryName));
            Assert.True(summary.OrderCount > 0);
            Assert.True(summary.AverageSum > 0);
            Assert.True(summary.TotalSum > 0);
        });

        Assert.Equal(
            expectedCategories.OrderBy(name => name),
            result.Select(summary => summary.CategoryName).OrderBy(name => name));
    }

    [Fact]
    public void GetTopSpenderClient_WhenOrdersExist_ReturnsClientWithMaxTotalAmount()
    {
        // Arrange
        var expectedTopSpender = _seeder.Orders
            .Where(o => o.Client != null)
            .GroupBy(o => o.Client!.Id)
            .Select(g => new
            {
                ClientId = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .ThenBy(x => x.ClientId)
            .First();

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
        Assert.Equal(expectedTopSpender.ClientId, result.Client.Id);
        Assert.Equal(expectedTopSpender.TotalSpent, result.TotalSpent);
    }
}
