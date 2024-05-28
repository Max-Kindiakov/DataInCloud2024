using DataInCloud.Dal.Car;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.Car
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
            var carToGet = new CarDao
            {
                Id = 100001,
                Name = "name",
                DoorsCount = 1,
                IsBuyEnable = false
            };
            
            await AppDbContext.AddAsync(carToGet);
            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, $"api/v1/cars/{carToGet.Id}");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
