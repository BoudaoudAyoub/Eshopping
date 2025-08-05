using Eshopping.API.Application.Commands;
using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshopping.API.Application.CommandsValidations;

public class OrderDeleteCommandValidator : OrderCommandValidator<OrderDeleteCommand, bool>
{
    public OrderDeleteCommandValidator(IOrderRepository orderRepository)
    {
        ValidateId(orderRepository);
    }
}