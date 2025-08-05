using Autofac;
using Autofac.Extensions.DependencyInjection;
using Eshopping.API.Configuration.Database;
using Eshopping.API.Configuration.Filters;
using Eshopping.API.Configuration.Modules;
using Eshopping.API.Configuration.SeedData;
using Eshopping.API.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

builder.Services.AddControllers(x =>
{
    x.Filters.Add<CustomExceptionFilter>();
});

builder.Services.AddSwaggerGen();

builder.Services.AddDatabaseSetup(builder.Configuration);

builder.Services.AutoMapperProfileRegistration();

builder.Host
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureContainer<ContainerBuilder>(container =>
       {
           container.RegisterModule<MediatorModule>();
           container.RegisterModule<ApplicationModule>();
       });

builder.Services.AddHostedService<OrderStatusUpdateBackgroundService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedData.Seeder(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseCors(b => b.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();