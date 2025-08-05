using Eshopping.Domain.Enums;
using Eshopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshopping.API.HostedServices;

/// <summary>
/// This background service is responsible for asynchronously updating order statuses at runtime
/// For better scalability, separation of concerns, and maintainability, consider hosting it as a separate worker service
/// outside the main API project in a production environment
/// </summary>
/// <param name="serviceProvider">The service provider used to create scoped dependencies</param>
public class OrderStatusUpdateBackgroundService(IServiceProvider serviceProvider) : BackgroundService

{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromMinutes(1);

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateOrders(stoppingToken);

            await Task.Delay(delayTime, stoppingToken);
        }
    }

    private async Task UpdateOrders(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<EshoppingContext>();

            List<Order> pendingOrders = await dbContext.Orders.Where(o => o.Status.Contains(OrderStatus.Pending.ToString())).ToListAsync(stoppingToken);

            if (pendingOrders.Count == 0) return;

            foreach (var order in pendingOrders)
            {
                order.UpdateStatus(OrderStatus.Shipped.ToString());
            }

            //TODO: raise an event related to shipping proccess
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            // TODO: log the errors or raise an event
            Console.WriteLine($"Error updating orders: {ex.Message}");
        }
    }
}