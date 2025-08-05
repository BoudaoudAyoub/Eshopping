using System.Diagnostics;
using Eshopping.Domain.Constants;
using Eshopping.Domain.Exceptions;
using Eshopping.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
namespace Eshopping.API.Configuration.SeedData;

public static partial class SeedData
{
    public static async Task Seeder(IServiceProvider serviceProvider)
    {
        int currentAttempt = 0;
        int maxAttempts = 5;
        bool isSucceed = false;
        TimeSpan delay = TimeSpan.FromSeconds(3);

        while (!isSucceed && maxAttempts > 0)
        {
            currentAttempt++;

            try
            {
                isSucceed = await SeedDataAsync(serviceProvider, isSucceed);
            }
            catch (SqlException ex)
            {
                maxAttempts--;
                Debug.WriteLine($"Database connection failed in attempt number : {currentAttempt++}): {ex.Message}");
                Thread.Sleep(delay);
            }
        }
    }

    private static async Task<bool> SeedDataAsync(IServiceProvider serviceProvider, bool isSucceed)
    {
        EshoppingContext eshoppingContext = serviceProvider.GetRequiredService<EshoppingContext>();

        RelationalDatabaseCreator dbCreator = eshoppingContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator ??
            throw new DomainException("Service unavailble...");

        if (!dbCreator.Exists() || eshoppingContext.Database.GetPendingMigrations().Any())
        {
            eshoppingContext.Database.Migrate();
        }

        await eshoppingContext.StartTransactionAsync();

        try
        {
            CreateUserIfNotExist(eshoppingContext);
            SeedProductsAndCategories(serviceProvider.GetRequiredService<IConfiguration>(), eshoppingContext);

            await eshoppingContext.Database.CommitTransactionAsync();

            isSucceed = true;
        }
        catch
        {
            eshoppingContext.Database.RollbackTransaction();
            // TODO: Implement detailed logging for better tracking and debugging
            throw new DomainException(ErrorConstants.ProductIssueMessage);
        }

        return isSucceed;
    }
}