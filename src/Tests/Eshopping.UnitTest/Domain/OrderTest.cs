using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Enums;
using Eshopping.Domain.Exceptions;
using Xunit;

namespace Eshopping.UnitTest.Domain;

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
    }

    [Fact]
    public void AddItems_ShouldThrow_WhenListIsNull()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act && Assert
        Assert.ThrowsAny<DomainException>(() => order.AddItems(null!));
    }
}