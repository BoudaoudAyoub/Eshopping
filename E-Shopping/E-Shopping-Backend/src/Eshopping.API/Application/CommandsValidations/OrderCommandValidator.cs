using FluentValidation;
using Eshopping.API.Application.Commands;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Constants;
using Eshopping.Domain.Enums;
namespace Eshopping.API.Application.CommandsValidations;

/// <summary>
/// A temporary model containing only the 'ProductId', 'ProductName' and 'Quantity' fields, 
/// used to avoid retrieving all columns from the product table
/// </summary>
/// <param name="ProductId">The unique identifier of the product</param>
/// <param name="ProductName">The product name</param>
/// <param name="Quantity">The quantity of the product</param>
public record ProductTempModel(
    Guid ProductId,
    string ProductName,
    int Quantity
);

public class OrderCommandValidator<TRequest, TResponse> : AbstractValidator<TRequest> where TRequest : OrderCommand<TResponse>
{
    public OrderCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected void ValidateId(IOrderRepository orderRepository) => RuleFor(command => command).Custom(async (command, context) =>
    {
        Order order = orderRepository.GetByIdAsync(command.Id, CancellationToken.None).Result;

        if (order is null)
        {
            context.AddFailure(ErrorConstants.InvalidOrderId);
            return;
        }

        string[] requests = [.. Enum.GetNames<OrderRequest>()];
        string[] status = Enum.GetNames<OrderStatus>().Where(name => !name.Contains(OrderStatus.Pending.ToString())).ToArray();

        if (requests.Contains(command.RequestType.ToString()) && status.Contains(order.Status))
        {
            if (command.RequestType.Contains(OrderRequest.Delete.ToString()) && order.Status.Contains(OrderStatus.Cancelled.ToString())) return;

            context.AddFailure($"Cannot {command.RequestType} the order because its '{order.Status}'");
            return;
        }
    });

    protected void OrderItemsValidation(IProductRepository productRepository)
    {
        RuleFor(command => command.OrderItems).NotEmpty().WithMessage("Order must contain at least one item");

        RuleFor(command => command.OrderItems)
            .Custom((orderItems, context) =>
            {
                List<Guid> productIds = orderItems.Select(item => item.ProductId).Distinct().ToList();

                List<ProductTempModel> products = [.. productRepository.GetAllAsQueryable()
                                                                       .Where(p => productIds.Contains(p.ID))
                                                                       .Select(p => new ProductTempModel(p.ID, p.Name, p.StockQuantity))];

                if (products.Count != productIds.Count)
                {
                    context.AddFailure("OrderItems", "Some products in the order do not exist");
                    return;
                }

                orderItems.ForEach(item =>
                {
                    ProductTempModel product = products.First(p => p.ProductId == item.ProductId);

                    if (item.Quantity <= 0)
                    {
                        context.AddFailure("OrderItems", $"Product {product.ProductName} must have a quantity greater than zero");
                        return;
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        context.AddFailure("OrderItems", $"Insufficient stock for product {product.ProductName}. Available : {product.Quantity}, Requested : {item.Quantity}");
                    }
                });
            });
    }
}