using Eshopping.API.Configuration.Modules.AutoMapper;
namespace Eshopping.API.Configuration.Modules;

public static class AutoMapperLoad
{
    public static void AutoMapperProfileRegistration(this IServiceCollection services)
    {
        services.AddAutoMapper(e =>
        {
            e.AddProfile<AppUserProfile>();
            e.AddProfile<ProductProfile>();
            e.AddProfile<OrderProfile>();
        });
    }
}