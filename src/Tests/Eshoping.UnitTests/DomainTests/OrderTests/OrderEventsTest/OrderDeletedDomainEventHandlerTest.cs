using Eshopping.Domain.Aggregates.OrderAggregate.Events;
using Eshopping.Domain.Aggregates.OrderAggregate;
using FluentAssertions;
using Xunit;

namespace Eshoping.UnitTests.DomainTests.OrderTests.OrderEventsTest;

public class OrderDeletedDomainEventHandlerTest
{
    [Fact]
    public async Task OrderDeletedDomainEventHandler_ShouldCompleteSuccessfully()
    {
        // Arrange
        OrderDeletedDomainEventHandler handler = new();

        Order order = new(Guid.NewGuid());
        order.AddItems([new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 12, 5)]);

        OrderDeletedDomainEvent @event = new(order);

        // Act
        Func<Task> action = () => handler.Handle(@event, CancellationToken.None);

        // Assert
        await action.Should().NotThrowAsync();
    }
}