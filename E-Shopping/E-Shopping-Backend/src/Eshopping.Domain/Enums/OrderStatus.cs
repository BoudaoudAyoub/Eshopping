namespace Eshopping.Domain.Enums;
public enum OrderStatus
{
    Pending,
    Shipped,
    Cancelled
}

public enum OrderRequest
{
    Cancel,
    Delete
}