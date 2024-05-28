using Autofac;
using Azure.Messaging.ServiceBus;
using DataInCloud.Dal;
using DataInCloud.Dal.Car;
using DataInCloud.Dal.Shop;
using DataInCloud.Model.Car;
using DataInCloud.Model.CarsInShop;
using DataInCloud.Model.Shop;
using DataInCloud.Model.ShopStats;
using DataInCloud.Model.Storage;
using DataInCloud.Orchestrators;
using DataInCloud.Orchestrators.Car;
using DataInCloud.Orchestrators.CarsInShop;
using DataInCloud.Orchestrators.Shop;
using DataInCloud.Orchestrators.ShopStatsOrchestrator;
using DataInCloud.Orchestrators.Storage;
using DataInCloud.Platform.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataInCloud.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<GlobalErrorHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cars API v1"));

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();

        services.AddAutoMapper(config =>
        {
            config.AddProfile<DaoMapper>();
            config.AddProfile<OrchestratorMapper>();
        });

        ConfigureDb(services);
        SetEdgeCommunicationDependencies(services);
        ConfigureEdgeService(services);
    }

    protected virtual void ConfigureDb(IServiceCollection services)
    {
        services.AddSqlServer<AppDbContext>(_configuration.GetConnectionString("DefaultConnection"));
        services.AddCosmos<CosmosDbContext>(_configuration.GetConnectionString("CosmosConnection")!, "cloud-homework");
    }

    protected virtual void SetEdgeCommunicationDependencies(IServiceCollection services)
    {
        services.AddScoped<IBlobStorage, BlobStorage>();
        services.AddSingleton<BlobStorageConfig>();
    }

    protected virtual void ConfigureEdgeService(IServiceCollection services)
    {
        services.AddSingleton(sp => new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnectionString")));
        services.AddScoped<IPublisher, ShopStatsPublisher>();

        services.AddSingleton<ISubscriber>(sp =>
        {
            var subscriber = new ShopStatsSubscriber(sp.GetRequiredService<ServiceBusClient>());
            subscriber.SubscribeAsync().GetAwaiter().GetResult();
            return subscriber;
        });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<CarOrchestrator>().As<ICarOrchestrator>();
        builder.RegisterType<CarRepository>().As<ICarRepository>();

        builder.RegisterType<ShopOrchestrator>().As<IShopOrchestrator>();
        builder.RegisterType<ShopRepository>().As<IShopRepository>();

        builder.RegisterType<CarsInShopOrchestrator>().As<ICarsInShopOrchestrator>();

        builder.RegisterType<ShopStatsOrchestrator>().As<IShopStatsOrchestrator>();
    }
}