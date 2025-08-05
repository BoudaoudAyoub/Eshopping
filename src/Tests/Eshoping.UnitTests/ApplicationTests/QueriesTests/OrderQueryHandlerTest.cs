using Moq;
using Eshopping.Infrastructure;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Domain.Enums;
using Eshopping.API.Application.Queries.OrderQueries;
using Eshopping.API.ViewModels;
using Xunit;
namespace Eshoping.UnitTests.ApplicationTests.QueriesTests;

public class OrderQueryHandlerTest
{
    private readonly Guid UserId = Guid.NewGuid();

    [Fact]
    public async Task OrderQueryHandler__ShouldReturn__OrdersList()
    {
        var options = new DbContextOptionsBuilder<EshoppingContext>()
                            .UseInMemoryDatabase(databaseName: "eshoppingContext")
                            .Options;

        EshoppingContext eshoppingContext = new(options, default!);

        if (eshoppingContext.Database.IsInMemory())
        {
            eshoppingContext.Users.AddRange(GetAppUsersQuery());
            eshoppingContext.SaveChanges();
        }
        
        Mock<IOrderRepository> _orderRepository = new();

        _orderRepository.Setup(o => o.GetAllAsQueryable()).Returns(OrdersQuery);

        OrderQueryHandler handler = new(_orderRepository.Object, eshoppingContext);

        List<OrderViewModel> orders = await handler.Handle(new OrderQuery(), CancellationToken.None);

        Assert.Single(orders);
        Assert.Contains("Ayoub", orders[0].Client);
        eshoppingContext.Dispose();
    }

    private IQueryable<AppUser> GetAppUsersQuery()
    {
        AppUser appUser = new("Ayoub", "Boudaoud", "ayoub@admin.com");
        appUser.SetId(UserId);

        return new List<AppUser> { appUser }.AsQueryable();
    }

    private IQueryable<Order> OrdersQuery()
    {
        Order order = new(UserId);

        OrderItem orderItem = new(Guid.NewGuid(), order.ID, 180, 12);
        orderItem.AddProduct(new Product(Guid.NewGuid(), "AE123", "Test", "Description", 120, 12)); 

        order.AddItems([orderItem]);

        return (new List<Order> { order }).AsQueryable();
    }
}