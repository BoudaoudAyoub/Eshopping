using Eshopping.Domain.Aggregates.UserAggregate;
using Eshopping.Domain.Enums;
using Eshopping.Domain.Seedwork;
namespace Eshopping.Domain.Aggregates.OrderAggregate;

public class Order(Guid userId) : Entity<Guid>
{
    public Guid UserId { get; private set; } = userId;
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
    public string Status { get; private set; } = OrderStatus.Pending.ToString();
    public bool IsShipped { get; private set; } = false;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public decimal TotalAmount => _orderItems.Sum(i => i.Total);
    public AppUser User { get; private set; } = default!;

    public void AddItems(List<OrderItem> orderItems)
    {
        ArgumentNullException.ThrowIfNull(orderItems);
        _orderItems.AddRange(orderItems);
    }

    public void UpdateStatus(string status)
    {
        if (!Enum.GetNames<OrderStatus>().Contains(status))
            throw new ArgumentException("Invalid status");
        Status = status;
    }
}