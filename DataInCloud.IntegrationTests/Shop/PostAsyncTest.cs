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
    public class PostAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;


        public PostAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }


        [Fact]
        public async Task PostAsync_TooBigPlacesAmount_ReturnsBadRequest()
        {
            //Arrange
            var inputModel = new CreateShop
            {
                Name = "name",
                PlacesAmount = 102,
            };


            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/shops");
            message.Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task PostAsync_TooBigName_ReturnsBadRequest()
        {
            //Arrange
            var inputModel = new CreateShop
            {
                Name = new string('m', 258),
                PlacesAmount = 1,
            };


            await CosmosDbContext.SaveChangesAsync();


            //Act
            var message = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/shops");
            message.Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);


            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task PostAsync_ValidInput_ReturnsCreatedEntity()
        {
            //Arrange
            var inputModel = new CreateShop
            {
                Name = "name",
                PlacesAmount = 1,
            };




            //Act
            var message = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/shops");
            message.Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);
            var actualResult = await response.Content.ReadFromJsonAsync<Orchestrators.Shop.Contract.Shop>();




            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            actualResult.Name.Should().Be(inputModel.Name);
            actualResult.PlacesAmount.Should().Be(inputModel.PlacesAmount);
            actualResult.Id.Should().NotBe(Guid.Empty);
        }
    }
}