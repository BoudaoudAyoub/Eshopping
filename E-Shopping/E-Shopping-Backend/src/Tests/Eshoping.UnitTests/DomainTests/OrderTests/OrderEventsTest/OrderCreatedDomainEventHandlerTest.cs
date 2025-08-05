using Eshopping.Domain.Aggregates.OrderAggregate.Events;
using Eshopping.Domain.Aggregates.OrderAggregate;
using FluentAssertions;
using Xunit;

namespace Eshoping.UnitTests.DomainTests.OrderTests.OrderEventsTest;

public class OrderCreatedDomainEventHandlerTest
{
    [Fact]
    public async Task OrderCreatedDomainEventHandler_ShouldCompleteSuccessfully()
    {
        // Arrange
        OrderCreatedDomainEventHandler handler = new();

        Order order = new(Guid.NewGuid());
        order.AddItems([new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 12, 5)]);

        OrderCreatedDomainEvent @event = new(order);

        // Act
        Func<Task> action = () => handler.Handle(@event, CancellationToken.None);

        // Assert
        await action.Should().NotThrowAsync();
    }
}