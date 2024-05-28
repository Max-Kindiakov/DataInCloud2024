using Azure.Messaging.ServiceBus;
using DataInCloud.Api;
using DataInCloud.Dal;
using DataInCloud.Model.Storage;
using DataInCloud.Platform.ServiceBus;
using EntityFrameworkCore.Testing.Common.Helpers;
using EntityFrameworkCore.Testing.Moq.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.IntegrationTests;

public class TestStartup : Startup
{
    static Guid inputShopId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
    static int inputCarId_1 = 1;
    static int inputCarId_2 = 2;
    string relationFileName = $"{inputShopId:N}_{inputCarId_1}";
    List<string> listResult = new List<string>
            {
                "93094f7d-3926-483f-9f44-3e4cf28929a1",
                "93094f7d-3926-483f-9f44-3e4cf28929a2",
                "93094f7d-3926-483f-9f44-3e4cf28929a3",
            };

    static Model.Car.Car carById_1 = new Model.Car.Car
    {
        Id = inputCarId_1,
        Name = "Avto1",
        DoorsCount = 1,
        IsBuyEnable = true
    };

    static Model.Car.Car carById_2 = new Model.Car.Car
    {
        Id = inputCarId_2,
        Name = "Avto2",
        DoorsCount = 2,
        IsBuyEnable = true
    };

    static List<int> CarsListResult = new List<int>() { carById_1.Id, carById_2.Id };

    static Model.Shop.Shop ShopById = new Model.Shop.Shop
    {
        Id = inputShopId,
        Name = "Saloon1",
        PlacesAmount = 50
    };

    static Model.CarsInShop.ShopCar modelResult = new Model.CarsInShop.ShopCar
    {
        ShopId = inputShopId,
        CarId = inputCarId_1
    };


    public TestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env) { }

    protected override void SetEdgeCommunicationDependencies(IServiceCollection services)
    {
        var blobStorageMock = new Mock<IBlobStorage>();
        blobStorageMock
            .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
            .ReturnsAsync(false);
        blobStorageMock
             .Setup(rm => rm.PutContentAsync(relationFileName))
             .Returns(Task.FromResult(5));
        blobStorageMock
            .Setup(rm => rm.FindByShopAsync(inputShopId))
            .ReturnsAsync(CarsListResult);

        services.AddSingleton(blobStorageMock.Object);
    }

    protected override void ConfigureEdgeService(IServiceCollection services)
    {
        var serviceBusClientMock = new Mock<ServiceBusClient>();
        services.AddSingleton(serviceBusClientMock.Object);

        var publisherMock = new Mock<IPublisher>();
        publisherMock
                .Setup(rm => rm.PublishAsync(ShopById.Id))
                .Returns(Task.FromResult(5));
        services.AddSingleton(publisherMock.Object);

        var subscriberMock = new Mock<ISubscriber>();
        subscriberMock
            .Setup(rm => rm.Data)
            .Returns(listResult);

        services.AddSingleton(subscriberMock.Object);
    }

    protected override void ConfigureDb(IServiceCollection services)
    {
        var context = ConfigureDb<AppDbContext>().MockedDbContext;
        services.AddSingleton(c => context);

        var cosmosDbContext = ConfigureDb<CosmosDbContext>().MockedDbContext;
        services.AddSingleton(c => cosmosDbContext);
    }

    private IMockedDbContextBuilder<T> ConfigureDb<T>()
        where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContextToMock = (T)Activator.CreateInstance(typeof(T), options);
        return new MockedDbContextBuilder<T>()
            .UseDbContext(dbContextToMock)
            .UseConstructorWithParameters(options);
    }
}