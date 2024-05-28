using DataInCloud.Dal.Car;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.Car
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
            AppDbContext.Cars.Add(new CarDao
            {
                Name = "name",
                DoorsCount = 1,
                IsBuyEnable = false
            });
            
            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, $"api/v1/cars");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
