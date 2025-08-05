using Eshopping.API.Application.Commands;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.OrderAggregate.Events;
using Eshopping.Domain.Enums;
using Moq;
using Xunit;

namespace Eshoping.UnitTests.ApplicationTests.CommandsTests;
public class OrderDeleteCommandHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    public OrderDeleteCommandHandlerTest()
    {
        _orderRepositoryMock = new();
    }

    [Fact]
    public async Task OrderDeleteCommandHandler__ShouldSucceed()
    {
        Order order = new(Guid.NewGuid());

        OrderDeleteCommand command = new()
        {
            Id = Guid.NewGuid(),
            RequestType = OrderRequest.Delete.ToString(),
            OrderItems = default!
        };

        _orderRepositoryMock.Setup(o => o.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _orderRepositoryMock.Setup(o => o.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        OrderDeleteCommandHandler orderDeleteCommandHandler = new(_orderRepositoryMock.Object);

        bool hasBeenDeleted = await orderDeleteCommandHandler.Handle(command, CancellationToken.None);

        Assert.True(hasBeenDeleted);
        Assert.Contains(order.DomainEvents!, e => e is OrderDeletedDomainEvent);
        _orderRepositoryMock.Verify(o => o.RemoveSingle(order), Times.Once);
        _orderRepositoryMock.Verify(o => o.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);

    }
}