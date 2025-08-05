using Eshopping.Domain.Seedwork;
namespace Eshopping.Domain.Aggregates.OrderAggregate;

public interface IOrderRepository : IRepository<Order, Guid> { }