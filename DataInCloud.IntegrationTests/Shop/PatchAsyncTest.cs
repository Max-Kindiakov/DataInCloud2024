using DataInCloud.Dal.Shop;
using DataInCloud.Orchestrators.Shop.Contract;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace DataInCloud.IntegrationTests.Shop
{
    public class PatchAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;


        public PatchAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }


        [Fact]
        public async Task PatchAsync_ReturnsOk()
        {
            //Arrange
            var shopToEdit = new ShopDao
            {
                Id = Guid.NewGuid(),
                Name = "name",
                PlacesAmount = 2,
            };


            var changes = new EditShop
            {
                PlacesAmount = 43,
            };


            await CosmosDbContext.AddAsync(shopToEdit);
            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/shops/{shopToEdit.Id}");
            message.Content = new StringContent(JsonConvert.SerializeObject(changes), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);
            var actualResult = await response.Content.ReadFromJsonAsync<Orchestrators.Shop.Contract.Shop>();


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            actualResult.PlacesAmount.Should().Be(43);
        }
    }
}