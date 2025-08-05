using Eshopping.Domain.Seedwork;
namespace Eshopping.Domain.Aggregates.ProductAggregate;

public class ProductCategory(string name) : Entity<Guid>
{
    public string Name { get; set; } = name;
    public IReadOnlyCollection<Product> Products { get; set; } = default!;
}