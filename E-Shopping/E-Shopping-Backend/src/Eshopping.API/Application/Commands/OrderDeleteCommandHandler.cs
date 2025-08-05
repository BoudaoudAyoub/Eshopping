using Eshopping.Domain.Aggregates.OrderAggregate.Events;
using Eshopping.Domain.Aggregates.OrderAggregate;
using MediatR;
namespace Eshopping.API.Application.Commands;

public class OrderDeleteCommand : OrderCommand<bool> { }

public class OrderDeleteCommandHandler(IOrderRepository orderRepository) : IRequestHandler<OrderDeleteCommand, bool>
{
    public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        order.AddDomainEvent(new OrderDeletedDomainEvent(order));
        orderRepository.RemoveSingle(order);
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}