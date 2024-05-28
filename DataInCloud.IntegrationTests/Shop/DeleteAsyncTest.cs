using DataInCloud.Dal.Shop;
using FluentAssertions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace DataInCloud.IntegrationTests.Shop
{
    public class DeleteAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;


        public DeleteAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }


        [Fact]
        public async Task DeleteAsync_IfExists_RemovesFromDb()
        {
            //Arrange
            var shopToDelete = new ShopDao
            {
                Id = Guid.NewGuid(),
                Name = "name",
                PlacesAmount = 2,
            };


            await CosmosDbContext.Shops.AddAsync(shopToDelete);
            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/shops/{shopToDelete.Id}");
            var response = await _httpClient.SendAsync(message);


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            CosmosDbContext.Shops.FirstOrDefault(x => x.Id == shopToDelete.Id).Should().BeNull();
        }
    }
}