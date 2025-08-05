using System.Reflection;
using Eshopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Eshopping.Infrastructure.Repositories;
namespace Eshopping.API.Configuration.Database;

public static class DatabaseSetup
{
    public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<EshoppingContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("EshppingConnectionString"), sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("_eshoppingContextMigrationsHistory");
                sqlOptions.MigrationsAssembly(typeof(Repository<,>).GetTypeInfo().Assembly.GetName().Name);
            });
        });
    }
}