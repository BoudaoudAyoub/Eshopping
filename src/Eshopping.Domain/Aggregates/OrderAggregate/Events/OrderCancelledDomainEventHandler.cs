using MediatR;
namespace Eshopping.Domain.Aggregates.OrderAggregate.Events;

public class OrderCancelledDomainEvent(Order order) : INotification
{
    public Order Order { get; } = order;
}

public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    public Task Handle(OrderCancelledDomainEvent eventReq, CancellationToken cancellationToken)
    {
        var order = eventReq.Order;

        // TODO: Implement the logic to handle the order cancelation event, such as sending notifications, logging, etc
        Console.WriteLine($"Order : {order.ID} with total {order.TotalAmount} has been canceled");

        return Task.CompletedTask;
    }
}