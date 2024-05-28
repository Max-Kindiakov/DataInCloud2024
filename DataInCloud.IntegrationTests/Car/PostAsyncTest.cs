using DataInCloud.Orchestrators.Car.Contract;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.Car
{
    public class PostAsyncTest : BaseTest
    {
        private readonly HttpClient _httpClient;

        public PostAsyncTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task PostAsync_TooBigDoorsCount_ReturnsBadRequest()
        {
            //Arrange
            var inputModel = new CreateCar
            {

                Name = "name",
                DoorsCount = 102,
                IsBuyEnable = false
            };

            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Post, $"api/v1/cars");
            message.Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task PostAsync_TooBigName_ReturnsBadRequest()
        {
            //Arrange
            var inputModel = new CreateCar
            {

                Name = new string('m',258),
                DoorsCount = 1,
                IsBuyEnable = false
            };

            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Post, $"api/v1/cars");
            message.Content = new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
