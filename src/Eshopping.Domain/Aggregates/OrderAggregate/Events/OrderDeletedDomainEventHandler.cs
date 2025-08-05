using MediatR;
namespace Eshopping.Domain.Aggregates.OrderAggregate.Events;

public class OrderDeletedDomainEvent(Order order) : INotification
{
    public Order Order { get; } = order;
}

public class OrderDeletedDomainEventHandler : INotificationHandler<OrderDeletedDomainEvent>
{
    public Task Handle(OrderDeletedDomainEvent eventReq, CancellationToken cancellationToken)
    {
        var order = eventReq.Order;

        // TODO: Implement the logic to handle the order deletion event, such as sending notifications, logging, etc
        Console.WriteLine($"Order : {order.ID} has been deleted");

        return Task.CompletedTask;
    }
}