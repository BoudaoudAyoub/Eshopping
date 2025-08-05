using MediatR;
using Eshopping.Infrastructure;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Domain.Aggregates.OrderAggregate.Events;
namespace Eshopping.API.Application.Commands;

public class OrderCreateCommand : OrderCommand<bool> { }

public class OrderCreateCommandHandler(
    EshoppingContext eshoppingContext,
    IProductRepository productRepository,
    IOrderRepository orderRepository)
    : IRequestHandler<OrderCreateCommand, bool>
{
    public async Task<bool> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        List<Product> products = GetProducts(productRepository, request);

        Order order = new(eshoppingContext.Users.First().ID);

        order.AddItems(request.OrderItems.Select(item =>
        {
            Product product = products.First(p => p.ID == item.ProductId);
            product.ReduceProductStock(item.Quantity);
            return new OrderItem(item.ProductId, order.ID, product.Price, item.Quantity);

        }).ToList());

        order.AddDomainEvent(new OrderCreatedDomainEvent(order));

        productRepository.UpdateAll(products);

        await orderRepository.AddSingleAsync(order, cancellationToken);

        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }

    private static List<Product> GetProducts(IProductRepository productRepository, OrderCreateCommand request)
    {
        return [.. productRepository.GetAllAsQueryable()
                                     .Where(products => request.OrderItems.Select(item => item.ProductId).Contains(products.ID))];
    }
}