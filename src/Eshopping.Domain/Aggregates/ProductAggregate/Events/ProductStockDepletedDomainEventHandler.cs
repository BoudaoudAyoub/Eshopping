using MediatR;
namespace Eshopping.Domain.Aggregates.ProductAggregate.Events;

public class ProductStockDepletedDomainEvent(Guid id) : INotification
{
    public Guid Id { get; set; } = id;
}

public class ProductStockDepletedDomainEventHandler(IProductRepository productRepository) : INotificationHandler<ProductStockDepletedDomainEvent>
{
    public async Task Handle(ProductStockDepletedDomainEvent eventReq, CancellationToken cancellationToken)
    {
        Product product = await productRepository.GetByIdAsync(eventReq.Id, cancellationToken);
        product.StockQuantity = product.QuantityBackup;
        productRepository.UpdateSingle(product);
        await productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}