using DataInCloud.Model.Car;
using DataInCloud.Model.Shop;
using DataInCloud.Model.Storage;
using DataInCloud.Orchestrators.CarsInShop;
using FluentAssertions;
using Moq;

namespace DataInCloud.Orchestrators.Tests
{
    public class CarInShopOrchestratorTest
    {
        static Guid inputShopId = Guid.NewGuid();
        static int inputCarId_1 = 1;
        static int inputCarId_2 = 2;
        string relationFileName = $"{inputShopId:N}_{inputCarId_1}";

        static Model.Car.Car carById_1 = new Model.Car.Car
        {
            Id = inputCarId_1,
            Name = "Car1",
            DoorsCount = 3,
            IsBuyEnable = true
        };

        static Model.Car.Car carById_2 = new Model.Car.Car
        {
            Id = inputCarId_2,
            Name = "Car2",
            DoorsCount = 5,
            IsBuyEnable = false
        };

        static List<int> carsListResult = new List<int>() { carById_1.Id, carById_2.Id };

        static Model.Shop.Shop shopById = new Model.Shop.Shop
        {
            Id = inputShopId,
            Name = "Shop1",
            PlacesAmount = 50
        };

        static Model.CarsInShop.ShopCar modelResult = new Model.CarsInShop.ShopCar
        {
            ShopId = inputShopId,
            CarId = inputCarId_1
        };

        [Fact]
        public async Task CreateAsync_IfNoException_StoresEntityAndReturnsResult()
        {
            //Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
                .ReturnsAsync(false);
            blobStorageMock
                .Setup(rm => rm.PutContentAsync(relationFileName))
                .Returns(Task.CompletedTask);

            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            carOrchestratorMock
                .Setup(rm => rm.GetCarAsync(inputCarId_1))
                .ReturnsAsync(carById_1);

            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            shopOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputShopId))
                .ReturnsAsync(shopById);

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                    carOrchestratorMock.Object,
                    shopOrchestratorMock.Object,
                    blobStorageMock.Object);

            //Act
            var createResult = await carInShopOrchestrator.CreateAsync(inputShopId, inputCarId_1);

            //Assert
            blobStorageMock.Verify(sar => sar.PutContentAsync(relationFileName), Times.Once);
            createResult.ShopId.Should().Be(modelResult.ShopId);
            createResult.CarId.Should().Be(modelResult.CarId);
        }

        [Fact]
        public async Task GetCarsAsync_IfNoException_GetAllEntityAndReturnsResult()
        {
            //Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.FindByShopAsync(inputShopId))
                .ReturnsAsync(carsListResult);

            var carOrchestratorMock = new Mock<ICarOrchestrator>();

            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            shopOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputShopId))
                .ReturnsAsync(shopById);

            var carsInShopOrchestrator = new CarsInShopOrchestrator(
                    carOrchestratorMock.Object,
                    shopOrchestratorMock.Object,
                    blobStorageMock.Object);

            //Act
            List<int> GetCarsAsyncResult = await carsInShopOrchestrator.GetCarsAsync(inputShopId);

            //Assert
            GetCarsAsyncResult.Should().NotBeEmpty();
            GetCarsAsyncResult.Should().Equal(carsListResult);
            GetCarsAsyncResult.Count.Should().Be(carsListResult.Count);
        }

        [Fact]
        public async Task CreateAsync_WithNonExistingCar_ThrowsArgumentException()
        {
            // Arrange
            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            carOrchestratorMock
                .Setup(rm => rm.GetCarAsync(inputCarId_1))
                .ReturnsAsync((Model.Car.Car)null); // Simulate non-existing car

            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            var blobStorageMock = new Mock<IBlobStorage>();

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await carInShopOrchestrator.CreateAsync(inputShopId, inputCarId_1));
        }

        [Fact]
        public async Task CreateAsync_WithNonExistingShop_ThrowsArgumentException()
        {
            // Arrange
            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            carOrchestratorMock
                .Setup(rm => rm.GetCarAsync(inputCarId_1))
                .ReturnsAsync(carById_1);

            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            shopOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputShopId))
                .ReturnsAsync((Model.Shop.Shop)null);

            var blobStorageMock = new Mock<IBlobStorage>();

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await carInShopOrchestrator.CreateAsync(inputShopId, inputCarId_1));
        }

        [Fact]
        public async Task CreateAsync_WithInvalidCarId_ThrowsArgumentException()
        {
            // Arrange
            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            var blobStorageMock = new Mock<IBlobStorage>();

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await carInShopOrchestrator.CreateAsync(inputShopId, -1));
        }

        [Fact]
        public async Task CreateAsync_WithEmptyShopId_ThrowsArgumentException()
        {
            // Arrange
            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            var blobStorageMock = new Mock<IBlobStorage>();

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await carInShopOrchestrator.CreateAsync(Guid.Empty, inputCarId_1));
        }

        [Fact]
        public async Task CreateAsync_WhenStorageThrowsException_ThrowsInvalidOperationException()
        {
            // Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
                .ReturnsAsync(false);
            blobStorageMock
                .Setup(rm => rm.PutContentAsync(relationFileName))
                .ThrowsAsync(new Exception("Storage error"));

            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            carOrchestratorMock
                .Setup(rm => rm.GetCarAsync(inputCarId_1))
                .ReturnsAsync(carById_1);

            var shopOrchestratorMock = new Mock<IShopOrchestrator>();
            shopOrchestratorMock
                .Setup(rm => rm.GetByIdAsync(inputShopId))
                .ReturnsAsync(shopById);

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await carInShopOrchestrator.CreateAsync(inputShopId, inputCarId_1));
        }

        [Fact]
        public async Task DeleteAsync_IfFileExists_DeletesFile()
        {
            // Arrange
            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(rm => rm.ContainsFileByNameAsync(relationFileName))
                .ReturnsAsync(true);
            blobStorageMock
                .Setup(rm => rm.DeleteAsync(relationFileName))
                .Returns(Task.CompletedTask);

            var carOrchestratorMock = new Mock<ICarOrchestrator>();
            var shopOrchestratorMock = new Mock<IShopOrchestrator>();

            var carInShopOrchestrator = new CarsInShopOrchestrator(
                carOrchestratorMock.Object,
                shopOrchestratorMock.Object,
                blobStorageMock.Object);

            // Act
            await carInShopOrchestrator.DeleteAsync(inputShopId, inputCarId_1);

            // Assert
            blobStorageMock.Verify(sar => sar.DeleteAsync(relationFileName), Times.Once);
        }
    }
}