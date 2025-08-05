using MediatR;
namespace Eshopping.API.Application.Commands;

public class OrderCommand<TReponse> : IRequest<TReponse>
{
    public Guid Id { get; set; }
    public string RequestType { get; set; } = string.Empty; // Canceled, Deleted... Could be others
    public List<OrderItemModel> OrderItems { get; set; } = [];  
}

public class OrderItemModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
