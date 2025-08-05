using Eshopping.Domain.Seedwork;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.ProductAggregate.Events;
namespace Eshopping.Domain.Aggregates.ProductAggregate;

public class Product : Entity<Guid>
{
    public Product() { }
    public Product(Guid productCategoryId, string referenceNumber, string name, string desciption, decimal price, int stockQuantity)
    {
        ProductCategoryId = productCategoryId;
        ReferenceNumber = referenceNumber;
        Name = name;
        Description = desciption;
        Price = price;
        StockQuantity = stockQuantity;
        QuantityBackup = stockQuantity;
    }

    public Guid ProductCategoryId { get; set; }
    public string ReferenceNumber { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int QuantityBackup { get; set; }

    public ProductCategory ProductCategory { get; private set; } = default!;
    public IReadOnlyCollection<OrderItem> OrderItems { get; set; } = default!;

    public void AddProductCategory(ProductCategory productCategory)
    {
        ArgumentNullException.ThrowIfNull(productCategory);

        ProductCategory = productCategory;
        ProductCategoryId = productCategory.ID;
    }

    public void ReduceProductStock(int quantity)
    {
        if (quantity == 0) return;
        StockQuantity -= quantity;

        if(StockQuantity == 0) AddDomainEvent(new ProductStockDepletedDomainEvent(ID));
    }
}