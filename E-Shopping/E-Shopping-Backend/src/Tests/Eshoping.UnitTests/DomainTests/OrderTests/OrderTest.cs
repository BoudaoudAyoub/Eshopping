using Xunit;
using Eshopping.Domain.Enums;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Constants;
namespace Eshoping.UnitTests.DomainTests.OrderTests;

public class OrderTest
{
    private readonly Guid UserId = Guid.NewGuid();

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Act
        var order = new Order(UserId);

        // Assert
        Assert.Equal(UserId, order.UserId);
        Assert.Equal(OrderStatus.Pending.ToString(), order.Status);
        Assert.False(order.IsShipped);
    }

    [Fact]
    public void AddItems_ShouldAddOrderItems()
    {
        Guid productId = Guid.NewGuid();
        Guid orderId = Guid.NewGuid();

        // Arrange
        var order = new Order(UserId);

        // Act
        order.AddItems(
        [
            new(productId, orderId, 12, 5),
            new(productId, orderId, 12, 5),
            new(productId, orderId, 12, 5)
        ]);

        // Assert
        Assert.NotEmpty(order.OrderItems);
        Assert.IsNotType<OrderItem>(order.OrderItems, exactMatch: false);
        Assert.Equal(12, order.OrderItems.First().UnitPrice);

        Assert.Equal(Math.Round(12 * 5 + SystemConstants.SHIPPING + SystemConstants.TAX, 2, MidpointRounding.AwayFromZero), 
            order.OrderItems.First().Total);
    }

    [Fact]
    public void AddItems_ShouldThrow_WhenListIsNull()
    {
        // Arrange
        var order = new Order(UserId);

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => order.AddItems(null!));
    }

    [Fact]
    public void UpdateStatus_ShouldThrow_WhenInvalid()
    {
        // Arrange
        var order = new Order(UserId);

        // Act  && Assert
        Assert.Throws<ArgumentException>(() => order.UpdateStatus("INVALID_STATUS"));
    }
}