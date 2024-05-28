using DataInCloud.Dal.Shop;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;


namespace DataInCloud.IntegrationTests.Shop
{
    public class GetByIdAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;


        public GetByIdAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }


        [Fact]
        public async Task GetByIdAsync_ReturnOneEntity()
        {
            //Arrange
            var shopToGet = new ShopDao
            {
                Id = Guid.NewGuid(),
                Name = "name",
                PlacesAmount = 2,
            };


            await CosmosDbContext.AddAsync(shopToGet);
            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/shops/{shopToGet.Id}");
            var response = await _httpClient.SendAsync(message);
            var actualResult = await response.Content.ReadFromJsonAsync<Orchestrators.Shop.Contract.Shop>();


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            actualResult.Id.Should().Be(shopToGet.Id);
        }
    }
}