using Eshopping.Domain.Seedwork;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Domain.Constants;
namespace Eshopping.Domain.Aggregates.OrderAggregate;

public class OrderItem(Guid productId, Guid orderId, decimal unitPrice, int quantity) : Entity<Guid>
{
    public Guid ProductId { get; private set; } = productId;
    public Guid OrderId { get; private set; } = orderId;
    public decimal UnitPrice { get; private set; } = unitPrice;
    public int Quantity { get; private set; } = quantity;
    public decimal Total 
    {
        get
        {
            decimal subtotal = UnitPrice * Quantity;
            decimal total = subtotal + SystemConstants.SHIPPING + SystemConstants.TAX;
            return Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }
    }
    public Product Product { get; private set; } = default!;
    public Order Order { get; private set; } = default!;

    public void AddProduct(Product product) => Product = product;
}