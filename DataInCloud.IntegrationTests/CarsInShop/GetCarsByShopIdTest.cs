using DataInCloud.Dal;
using Newtonsoft.Json;
using FluentAssertions;
using Xunit;
using DataInCloud.Dal.Shop;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DataInCloud.IntegrationTests.CarsInShop
{
    public class GetCarsByShopIdTest : BaseTest
    {
        private readonly HttpClient _httpClient;
        public GetCarsByShopIdTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetCarsByShopId_IfNoExceptionOccurred_GetAllEntityFromDb()
        {
            // Arrange
            Guid inputShopId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
            int inputCarId_1 = 1;
            int inputCarId_2 = 2;

            Model.Car.Car carById_1 = new Model.Car.Car
            {
                Id = inputCarId_1,
                Name = "Avto1",
                DoorsCount = 1
            };

            Model.Car.Car carById_2 = new Model.Car.Car
            {
                Id = inputCarId_2,
                Name = "Avto2",
                DoorsCount = 2
            };

            List<int> CarsListResult = new List<int>() { carById_1.Id, carById_2.Id };

            Model.Shop.Shop ShopById = new Model.Shop.Shop
            {
                Id = inputShopId,
                Name = "Saloon1",
                PlacesAmount = 50
            };

            await using var contextShop = Host.Services.GetService<CosmosDbContext>();
            var postShopResult = await contextShop.Shops.AddAsync(new ShopDao
            {
                Id = inputShopId,
                Name = ShopById.Name,
                PlacesAmount = ShopById.PlacesAmount,

            });
            await contextShop.SaveChangesAsync();

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/v1/shops/{inputShopId}/cars");

            var getCarsByShopIdResult = await _httpClient.SendAsync(message);

            //Assert
            getCarsByShopIdResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            getCarsByShopIdResult.Content.Should().NotBeNull();
            var getCarsByShopIdResultResponseModel = JsonConvert.DeserializeObject<List<int>>(
                await getCarsByShopIdResult.Content.ReadAsStringAsync());
            getCarsByShopIdResultResponseModel.Should().NotBeNull();
            getCarsByShopIdResultResponseModel.Count.Should().Be(CarsListResult.Count);
            getCarsByShopIdResultResponseModel.Should().Equal(CarsListResult);
        }
    }
}
