using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshopping.Infrastructure.Repositories;

public class OrderRepository(EshoppingContext context) : Repository<Order, Guid>(context), IOrderRepository { }