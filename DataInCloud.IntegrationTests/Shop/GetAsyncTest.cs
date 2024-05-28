using DataInCloud.Dal.Shop;
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;


namespace DataInCloud.IntegrationTests.Shop
{
    public class GetAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;


        public GetAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }


        [Fact]
        public async Task GetAsync_ReturnAllEntities()
        {
            //Arrange
            CosmosDbContext.Shops.Add(new ShopDao
            {
                Name = "name",
                PlacesAmount = 1,
            });


            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/shops");
            var response = await _httpClient.SendAsync(message);
            var actualResult = await response.Content.ReadFromJsonAsync<List<Orchestrators.Shop.Contract.Shop>>();


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            actualResult.Should().HaveCount(1);
        }
    }
}