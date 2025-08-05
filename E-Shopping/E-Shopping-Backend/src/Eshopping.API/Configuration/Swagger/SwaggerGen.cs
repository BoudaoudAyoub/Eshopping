using Microsoft.OpenApi.Models;
namespace Eshopping.API.Configuration.Swagger;

public static class SwaggerGen
{
    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "E-Shopping API",
                Version = "v1"
            });
        });
    }
}