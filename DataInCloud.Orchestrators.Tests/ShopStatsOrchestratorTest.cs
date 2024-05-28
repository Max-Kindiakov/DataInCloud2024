using DataInCloud.Platform.ServiceBus;
using FluentAssertions;
using Moq;


namespace DataInCloud.Orchestrators.Test.ShopStats
{
    public class ShopStatsOrchestratorTest
    {
        [Fact]
        public async Task GetStatsAsync_IfNoException_GetAllEntityAndReturnsResult()
        {
            //Arrange
            List<string> listResult = new List<string>
            {
                "93094f7d-3926-483f-9f44-3e4cf28929a1",
                "93094f7d-3926-483f-9f44-3e4cf28929a2",
                "93094f7d-3926-483f-9f44-3e4cf28929a3",
            };

            var subscriberMock = new Mock<ISubscriber>();
            subscriberMock
                .Setup(rm => rm.Data)
                .Returns(listResult);

            var orchestrator = new ShopStatsOrchestrator.ShopStatsOrchestrator(subscriberMock.Object);

            //Act
            List<string> getStatsAsyncResult = await orchestrator.GetStatsAsync();

            //Assert
            getStatsAsyncResult.Should().NotBeEmpty();
            getStatsAsyncResult.Count.Should().Be(listResult.Count);
            getStatsAsyncResult.Should().Equal(listResult);
        }
    }
}