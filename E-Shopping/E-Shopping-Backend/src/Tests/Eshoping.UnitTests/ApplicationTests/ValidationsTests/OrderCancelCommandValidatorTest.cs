using Moq;
using Xunit;
using FluentValidation;
using Eshopping.API.Application.Commands;
using Eshopping.API.Application.CommandsValidations;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Constants;
using Eshopping.Domain.Enums;
namespace Eshoping.UnitTests.ApplicationTests.ValidationsTests;

public class OrderCancelCommandValidator : OrderCommandValidator<OrderCancelCommand, bool>
{
    private readonly Mock<IOrderRepository> _orderRepository;

    public OrderCancelCommandValidator(Order order)
    {
        _orderRepository = new();
        _orderRepository.Setup(o => o.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(order);
        ValidateId(_orderRepository.Object);
    }
}

public class OrderCancelCommandValidatorTest
{
    private readonly OrderCancelCommand _command;
    private IValidator<OrderCancelCommand> _validator = default!;

    public OrderCancelCommandValidatorTest()
    {
        _command = Mock.Of<OrderCancelCommand>();
    }

    [Fact]
    public void OrderCancelCommand__ValidateId__ShouldReturn_InvalidOrderId()
    {
        // Arrange
        _command.Id = Guid.NewGuid();

        // Act
        _validator = new OrderCancelCommandValidator(default!);
        var result = _validator.Validate(_command);

        //Assert
        Assert.True(result.Errors.ConvertAll(_ => _.ErrorMessage)?.Contains(ErrorConstants.InvalidOrderId));
    }

    [Fact]
    public void OrderCancelCommand__ValidateId__ShouldReturn_RequestCannotBeCanceled()
    {
        // Arrange
        _command.Id = Guid.NewGuid();
        _command.RequestType = OrderRequest.Cancel.ToString();

        Order order = new(Guid.NewGuid());
        order.UpdateStatus(OrderStatus.Cancelled.ToString());

        // Act
        _validator = new OrderCancelCommandValidator(order);
        var result = _validator.Validate(_command);

        //Assert
        Assert.True(result.Errors.ConvertAll(_ => _.ErrorMessage)?.Contains($"Cannot {OrderRequest.Cancel} the order because its '{order.Status}'"));
    }
}