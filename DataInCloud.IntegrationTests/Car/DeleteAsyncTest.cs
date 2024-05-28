using DataInCloud.Dal.Car;
using FluentAssertions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.Car
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
            var carToDelete = new CarDao
            {
                Id = 100001,
                Name = "name",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            await AppDbContext.Cars.AddAsync(carToDelete);
            await AppDbContext.SaveChangesAsync();

            //Act
            var message = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/cars/{carToDelete.Id}");
            var response = await _httpClient.SendAsync(message);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            AppDbContext.Cars.FirstOrDefault(x =>  x.Id == carToDelete.Id).Should().BeNull();
        }
    }
}
