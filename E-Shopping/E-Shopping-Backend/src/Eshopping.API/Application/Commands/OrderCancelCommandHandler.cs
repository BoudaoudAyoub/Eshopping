using MediatR;
using Eshopping.Domain.Enums;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.OrderAggregate.Events;
namespace Eshopping.API.Application.Commands;

public class OrderCancelCommand : OrderCommand<bool> { }

public class OrderCancelCommandHandler(IOrderRepository orderRepository) : IRequestHandler<OrderCancelCommand, bool>
{
    public async Task<bool> Handle(OrderCancelCommand request, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        order.UpdateStatus(OrderStatus.Cancelled.ToString());
        order.AddDomainEvent(new OrderCancelledDomainEvent(order));
        orderRepository.UpdateSingle(order);
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}