using MediatR;
using Eshopping.API.ViewModels;
using Eshopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.UserAggregate;
namespace Eshopping.API.Application.Queries.OrderQueries;

public class OrderQuery : IRequest<List<OrderViewModel>> { }

public class OrderQueryHandler(IOrderRepository orderRepository, EshoppingContext eshoppingContext) 
    : IRequestHandler<OrderQuery, List<OrderViewModel>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<List<OrderViewModel>> Handle(OrderQuery request, CancellationToken cancellationToken)
    {
        AppUser currentUser = eshoppingContext.Users.First();
        string currentUserFullName = string.Join(" ", currentUser.FirstName, currentUser.LastName);

        return await Task.FromResult(_orderRepository.GetAllAsQueryable()
                                                     .Where(o => o.UserId == currentUser.ID)
                                                     .Include(e => e.OrderItems)
                                                     .ThenInclude(e => e.Product)
                                                     .Select(o => new OrderViewModel()
                                                     {
                                                         Id = o.ID,
                                                         CreatedDate = o.OrderDate.ToString("yyyy-MM-dd"),
                                                         Status = o.Status.ToString(),
                                                         TotalAmount = o.TotalAmount,
                                                         Client = currentUserFullName,
                                                         IsSipped = o.IsShipped,
                                                         OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel()
                                                         {
                                                             ProductId = oi.ProductId,
                                                             Quantity = oi.Quantity,
                                                             Price = oi.UnitPrice,
                                                             ProductName = oi.Product.Name
                                                         }).ToList()
                                                     }).ToList());
    }
}