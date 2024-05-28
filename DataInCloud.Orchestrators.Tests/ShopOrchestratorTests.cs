using AutoMapper;
using DataInCloud.Api.Controllers;
using DataInCloud.Model.Shop;
using DataInCloud.Orchestrators.Shop.Contract;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataInCloud.Orchestrators.Tests
{
    public class ShopOrchestratorTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ShopsController>> _loggerMock;
        private readonly Mock<IShopOrchestrator> _orchestratorMock;
        private readonly ShopsController _controller;

        public ShopOrchestratorTests()
        {
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ShopsController>>();
            _orchestratorMock = new Mock<IShopOrchestrator>();
            _controller = new ShopsController(_mapperMock.Object, _loggerMock.Object, _orchestratorMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllShops()
        {
            // Arrange
            var shops = new List<Model.Shop.Shop> { new Model.Shop.Shop { Name = "Shop1" }, new Model.Shop.Shop { Name = "Shop2" } };
            _orchestratorMock.Setup(o => o.GetAllAsync()).ReturnsAsync(shops);

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(shops);
        }

        [Fact]
        public async Task GetById_ReturnsShopById()
        {
            // Arrange
            var shopId = Guid.NewGuid();
            var shop = new Model.Shop.Shop { Id = shopId, Name = "Shop1" };
            _orchestratorMock.Setup(o => o.GetByIdAsync(shopId)).ReturnsAsync(shop);

            // Act
            var result = await _controller.GetById(shopId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(shop);
        }

        [Fact]
        public async Task Post_CreatesShop()
        {
            // Arrange
            var createShop = new CreateShop { Name = "Shop1", PlacesAmount = 10 };
            var shop = new Model.Shop.Shop { Name = "Shop1", PlacesAmount = 10 };
            _mapperMock.Setup(m => m.Map<Model.Shop.Shop>(createShop)).Returns(shop);
            _orchestratorMock.Setup(o => o.CreateAsync(shop)).ReturnsAsync(shop);

            // Act
            var result = await _controller.PostSync(createShop);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(shop);
        }

        [Fact]
        public async Task Put_UpdatesShop()
        {
            // Arrange
            var shopId = Guid.NewGuid();
            var editShop = new EditShop { PlacesAmount = 20 };
            var shop = new Model.Shop.Shop { Id = shopId, Name = "Shop1", PlacesAmount = 20 };
            _mapperMock.Setup(m => m.Map<Model.Shop.Shop>(editShop)).Returns(shop);
            _orchestratorMock.Setup(o => o.UpdateAsync(shop)).ReturnsAsync(shop);

            // Act
            var result = await _controller.PutAsync(shopId, editShop);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(shop);
        }

        [Fact]
        public async Task Delete_DeletesShop()
        {
            // Arrange
            var shopId = Guid.NewGuid();
            var shop = new Model.Shop.Shop { Id = shopId, Name = "Shop1" };
            _orchestratorMock.Setup(o => o.DeleteAsync(shopId)).ReturnsAsync(shop);

            // Act
            var result = await _controller.DeleteAsync(shopId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(shop);
        }
    }
}