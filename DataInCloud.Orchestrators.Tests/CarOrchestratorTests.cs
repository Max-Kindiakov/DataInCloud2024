using DataInCloud.Model.Car;
using DataInCloud.Orchestrators.Car;
using FluentAssertions;
using Moq;

namespace DataInCloud.Orchestrators.Tests
{
    public class CarOrchestratorTests
    {
        [Fact]
        public async Task GetByIdAsync_IfExists_ReturnsCar()
        {
            //Arrange
            const int id = 100002;
            var existingCar = new Model.Car.Car
            {
                Id = id,
                Name = "Test",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            var repository = new Mock<ICarRepository>();
            repository.Setup(r => r.GetCarAsync(id)).ReturnsAsync(existingCar);

            var orchestrator = new CarOrchestrator(repository.Object);

            //Act
            var result = await orchestrator.GetCarAsync(id);

            //Assert
            result.Should().Be(existingCar);
        }

        [Fact]
        public async Task GetByIdAsync_IfNotExists_ReturnsNull()
        {
            //Arrange
            const int id = 100002;
            

            var repository = new Mock<ICarRepository>();
            repository.Setup(r => r.GetCarAsync(id)).ReturnsAsync((Model.Car.Car)null);

            var orchestrator = new CarOrchestrator(repository.Object);

            //Act
            var result = await orchestrator.GetCarAsync(id);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task EditAsync_IfExists_ReturnsCar()
        {
            //Arrange
            const int id = 100002;
            var existingCar = new Model.Car.Car
            {
                Id = id,
                Name = "Test",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            var repository = new Mock<ICarRepository>();
            repository.Setup(r => r.GetCarAsync(id)).ReturnsAsync(existingCar);
            repository.Setup(r => r.EditAsync(existingCar)).ReturnsAsync(existingCar);

            var orchestrator = new CarOrchestrator(repository.Object);

            //Act
            var result = await orchestrator.EditAsync(existingCar);

            //Assert
            result.Should().Be(existingCar);
        }


        [Fact]
        public async Task EditAsync_IfNotExists_ReturnsNull()
        {
            //Arrange
            const int id = 100002;
            var existingCar = new Model.Car.Car
            {
                Id = id,
                Name = "Test",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            var repository = new Mock<ICarRepository>();

            repository.Setup(r => r.EditAsync(existingCar)).ReturnsAsync((Model.Car.Car)null);

            var orchestrator = new CarOrchestrator(repository.Object);

            //Act
            var result = await orchestrator.EditAsync(existingCar);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_IfExists_ReturnsDeletedCar()
        {
            //Arrange
            const int id = 100002;
            var existingCar = new Model.Car.Car
            {
                Id = id,
                Name = "Test",
                DoorsCount = 1,
                IsBuyEnable = false
            };

            var repository = new Mock<ICarRepository>();

            repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(existingCar);

            var orchestrator = new CarOrchestrator(repository.Object);

            //Act
            var result = await orchestrator.DeleteAsync(id);

            //Assert
            result.Should().BeEquivalentTo(existingCar);
        }


        [Fact]
        public async Task DeleteAsync_IfNotExists_ReturnsNull()
        {
            // Arrange
            const int id = 100002;
            Model.Car.Car nonExistingCar = null;

            var repository = new Mock<ICarRepository>();
            repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(nonExistingCar);

            var orchestrator = new CarOrchestrator(repository.Object);

            // Act
            var result = await orchestrator.DeleteAsync(id);

            // Assert
            result.Should().BeNull();
        }

    }
}