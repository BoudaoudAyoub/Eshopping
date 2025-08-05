using Eshopping.Domain.Aggregates.OrderAggregate;

namespace Eshopping.API.ViewModels;

public class OrderItemCreateModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderViewModel
{
    public Guid Id { get; set; }
    public string Client { get; set; }
    public string CreatedDate { get; set; }
    public string Status { get; set; }
    public bool IsSipped { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; } = [];
}

public class OrderItemViewModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; } = string.Empty;
}

public class OrderFilterModel
{
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; }
}