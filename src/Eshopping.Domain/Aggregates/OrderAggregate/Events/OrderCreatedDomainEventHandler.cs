using MediatR;
namespace Eshopping.Domain.Aggregates.OrderAggregate.Events;

public class OrderCreatedDomainEvent(Order order) : INotification
{
    public Order Order { get; } = order;
}

public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    public Task Handle(OrderCreatedDomainEvent eventReq, CancellationToken cancellationToken)
    {
        var order = eventReq.Order;

        // TODO: Implement the logic to handle the order creation event, such as sending notifications, logging, etc
        Console.WriteLine($"Order created: {order.ID} with total {order.TotalAmount}");

        return Task.CompletedTask;
    }
}