using Eshopping.Domain.Aggregates.OrderAggregate;
using Eshopping.Domain.Aggregates.ProductAggregate;
using Eshopping.Domain.Aggregates.UserAggregate;
using Eshopping.Domain.Seedwork;
using Eshopping.Infrastructure.EntityConfigurations.AppUserConfigurations;
using Eshopping.Infrastructure.EntityConfigurations.OrderConfigurations;
using Eshopping.Infrastructure.EntityConfigurations.ProductConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace Eshopping.Infrastructure;

public class EshoppingContext(DbContextOptions<EshoppingContext> options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    public virtual DbSet<AppUser> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    private IDbContextTransaction _currentTransaction = default!;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction is not null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //entities configurations
        modelBuilder.ApplyConfiguration(new AppUserEntityTypeConfiguration())
                    .ApplyConfiguration(new OrderEntityTypeConfiguration())
                    .ApplyConfiguration(new OrderItemEntityTypeConfiguration())
                    .ApplyConfiguration(new ProductEntityTypeConfiguration())
                    .ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker.Entries<Entity<Guid>>()
                                          .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

        foreach (var entityEntry in domainEntities)
        {
            IReadOnlyCollection<INotification> events = [.. entityEntry.Entity.DomainEvents!];

            entityEntry.Entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }

    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        // If the given dbContextTransaction is not null then return the default value of "IDbContextTransaction" which is 'null'
        if (_currentTransaction is not null) return default!;

        // Initialize a new instance to the current transaction using Database.BeginTransactionAsync() EFC method
        _currentTransaction = await Database.BeginTransactionAsync();

        // Return the current transaction
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    private void DisposeTransaction()
    {
        if (_currentTransaction is null) return;
        _currentTransaction.Dispose();
        _currentTransaction = default!;
    }
}