using Eshopping.API.Application.Commands;
using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.API.Application.CommandsValidations;

public class OrderCreateCommandValidator : OrderCommandValidator<OrderCreateCommand, bool>
{
    public OrderCreateCommandValidator(IProductRepository productRepository)
    {
        OrderItemsValidation(productRepository);
    }
}