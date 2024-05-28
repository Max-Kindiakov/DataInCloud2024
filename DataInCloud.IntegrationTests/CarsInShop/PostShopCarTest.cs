using Newtonsoft.Json;
using DataInCloud.Dal;
using DataInCloud.Dal.Car;
using FluentAssertions;
using Xunit;
using DataInCloud.Dal.Shop;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DataInCloud.IntegrationTests.CarsInShop
{
    public class PostShopCarTest: BaseTest
    {
        private readonly HttpClient _httpClient;
        public PostShopCarTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PostAsync_IfNoExceptionOccurred_SavesNewEntityInDb()
        {
            // Arrange
            Guid inputShopId = Guid.Parse("93094f7d-3926-483f-9f44-3e4cf28929a2");
            int inputCarId_1 = 1;
            string relationFileName = $"{inputShopId:N}_{inputCarId_1}";

            Model.Car.Car carById_1 = new Model.Car.Car
            {
                Id = inputCarId_1,
                Name = "Avto1",
                DoorsCount = 1,
                IsBuyEnable = true
            };

            Model.Shop.Shop ShopById = new Model.Shop.Shop
            {
                Id = inputShopId,
                Name = "Saloon1",
                PlacesAmount = 50
            };

            Model.CarsInShop.ShopCar modelShopCarResult = new Model.CarsInShop.ShopCar
            {
                ShopId = inputShopId,
                CarId = inputCarId_1
            };

            await using var contextShop = Host.Services.GetService<CosmosDbContext>();
            var postShopResult = await contextShop.Shops.AddAsync(new ShopDao
            {
                Id = inputShopId,
                Name = ShopById.Name,
                PlacesAmount = ShopById.PlacesAmount
            });
            await contextShop.SaveChangesAsync();

            await using var contextCar = Host.Services.GetService<AppDbContext>();
            var postCarResult_1 = await contextCar.Cars.AddAsync(new CarDao
            {
                Name = carById_1.Name,
                DoorsCount = carById_1.DoorsCount,
                IsBuyEnable = carById_1.IsBuyEnable
            });
            await contextCar.SaveChangesAsync();

            // Act
            var message = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/v1/shops/{inputShopId}/cars/{inputCarId_1}");

            var postResult = await _httpClient.SendAsync(message);

            //Assert
            postResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            postResult.Content.Should().NotBeNull();
            var postResponseModel = JsonConvert.DeserializeObject<Model.CarsInShop.ShopCar>(
                await postResult.Content.ReadAsStringAsync());
            postResponseModel.Should().NotBeNull();
            postResponseModel.ShopId.Should().Be(modelShopCarResult.ShopId);
            postResponseModel.CarId.Should().Be(modelShopCarResult.CarId);
        }
    }
}
