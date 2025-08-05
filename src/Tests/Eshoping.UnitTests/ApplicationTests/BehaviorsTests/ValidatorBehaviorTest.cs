using MediatR;
using FluentValidation;
using Eshopping.API.Application.Behaviors;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Eshopping.Domain.Exceptions;
namespace Eshoping.UnitTests.ApplicationTests.BehaviorsTests;

public record MockRequest(Guid OrderId, string RequestType) : IRequest<bool>;

public class MockRequestValidator : AbstractValidator<MockRequest>
{
    public MockRequestValidator()
    {
        RuleFor(_ => _.RequestType).NotEqual(string.Empty).WithMessage("Request type should not be empty");
    }
}

public class ValidatorBehaviorTest
{
    private readonly ValidatorBehavior<MockRequest, bool> Behavior;
    private readonly ILogger<ValidatorBehavior<MockRequest, bool>> Logger;

    public ValidatorBehaviorTest()
    {
        var validators = new IValidator<MockRequest>[] { new MockRequestValidator() };

        var loggerMock = new Mock<ILogger<ValidatorBehavior<MockRequest, bool>>>();
        Logger = loggerMock.Object;

        Behavior = new ValidatorBehavior<MockRequest, bool>(Logger, validators);
    }

    [Fact]
    public async Task ValidatorBehavior__ShouldThrowsDomainException__WhenAllRequirementsAreInvalid()
    {
        var request = new MockRequest(Guid.Empty, string.Empty);

        await Assert.ThrowsAsync<DomainException>( async() => await Behavior.Handle(request, CancellationToken.None, () => Task.FromResult(true)));
    }

    [Fact]
    public async Task ValidatorBehavior__ShouldReturnTrue__WhenAllRequirementSucceed()
    {
        var request = new MockRequest(Guid.NewGuid(), "Cancel");

        bool nextHandlerCalled = false;

        await Behavior.Handle(request, CancellationToken.None, () => { nextHandlerCalled = true; return Task.FromResult(true); });

        Assert.True(nextHandlerCalled);
    }
}