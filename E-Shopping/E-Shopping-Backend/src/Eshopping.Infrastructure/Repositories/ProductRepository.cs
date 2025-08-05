using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.Infrastructure.Repositories;

public class ProductRepository(EshoppingContext context) : Repository<Product, Guid>(context), IProductRepository { }