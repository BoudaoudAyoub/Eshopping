using Eshopping.Domain.Aggregates.OrderAggregate.Events;
using Xunit;
using FluentAssertions;
using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshoping.UnitTests.DomainTests.OrderTests.OrderEventsTest;

public class OrderCancelledDomainEventHandlerTest
{
    [Fact]
    public async Task OrderCancelledDomainEventHandler_ShouldCompleteSuccessfully()
    {
        // Arrange
        OrderCancelledDomainEventHandler handler = new();

        Order order = new(Guid.NewGuid());
        order.AddItems([new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 12, 5)]);

        OrderCancelledDomainEvent @event = new(order);

        // Act
        Func<Task> action = () => handler.Handle(@event, CancellationToken.None);

        // Assert
        await action.Should().NotThrowAsync();
    }
}