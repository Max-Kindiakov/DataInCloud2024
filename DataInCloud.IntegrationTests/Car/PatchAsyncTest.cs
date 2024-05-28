using DataInCloud.Dal.Car;
using DataInCloud.Orchestrators.Car.Contract;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.Car
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
            var carToEdit = new CarDao
            {
                Id = 100001,
                Name = "name",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            var changes = new EditCar
            {
                Name = "name2024",
            };
            
            await AppDbContext.AddAsync(carToEdit);
            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/cars/{carToEdit.Id}");
            message.Content = new StringContent(JsonConvert.SerializeObject(changes), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
