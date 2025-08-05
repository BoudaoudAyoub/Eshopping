using MediatR;
using Eshopping.Infrastructure;
using Eshopping.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace Eshopping.API.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(EshoppingContext eshoppingContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest
    : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    private readonly EshoppingContext _eshoppingContext = eshoppingContext ?? throw new ArgumentException(nameof(EshoppingContext));

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        TResponse response = default!;

        string typeName = typeof(TRequest).Name;

        try
        {
            if (_eshoppingContext.HasActiveTransaction) await next();

            IExecutionStrategy strategy = _eshoppingContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _eshoppingContext.StartTransactionAsync();

                _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                response = await next();

                _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                await _eshoppingContext.CommitTransactionAsync(transaction);
            });

            return response;
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
            throw;
        }
    }
}