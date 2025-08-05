using Eshopping.API.Application.Commands;
using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshopping.API.Application.CommandsValidations;

public class OrderCancelCommandValidator : OrderCommandValidator<OrderCancelCommand, bool>
{
    public OrderCancelCommandValidator(IOrderRepository orderRepository)
    {
        ValidateId(orderRepository);
    }
}