using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DataInCloud.IntegrationTests.ShopStats
{
    public class GetAllStatsTest : BaseTest
    {
        private readonly HttpClient _httpClient;

        public GetAllStatsTest()
        {
            _httpClient = InitTestServer().GetClient();
        }

        [Fact]
        public async Task GetStatsAsync_IfNoExceptionOccurred_GetAllEntityFromServiceBus()
        {
            // Arrange
            var listResult = new List<string>
            {
                "93094f7d-3926-483f-9f44-3e4cf28929a1",
                "93094f7d-3926-483f-9f44-3e4cf28929a2",
                "93094f7d-3926-483f-9f44-3e4cf28929a3",
            };

            // Act
            var message = new HttpRequestMessage(HttpMethod.Get, "api/v1/stats");
            var getStatsAsyncResult = await _httpClient.SendAsync(message);

            //Assert
            getStatsAsyncResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            getStatsAsyncResult.Content.Should().NotBeNull();

            var getStatsAsyncResultModel = JsonConvert.DeserializeObject<List<string>>(await getStatsAsyncResult.Content.ReadAsStringAsync());

            getStatsAsyncResultModel.Should().NotBeEmpty();
            getStatsAsyncResultModel.Count.Should().Be(listResult.Count);
            getStatsAsyncResultModel.Should().Equal(listResult);
        }
    }
}