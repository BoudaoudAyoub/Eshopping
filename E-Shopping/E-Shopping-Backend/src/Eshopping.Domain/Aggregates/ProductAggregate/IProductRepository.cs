using Eshopping.Domain.Seedwork;
namespace Eshopping.Domain.Aggregates.ProductAggregate;

public interface IProductRepository : IRepository<Product, Guid> 
{
}