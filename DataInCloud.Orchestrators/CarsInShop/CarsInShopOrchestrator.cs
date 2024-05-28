using DataInCloud.Model.Car;
using DataInCloud.Model.CarsInShop;
using DataInCloud.Model.Shop;
using DataInCloud.Model.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.CarsInShop
{
    public class CarsInShopOrchestrator : ICarsInShopOrchestrator
    {
        private readonly ICarOrchestrator _carOrchestrator;
        private readonly IShopOrchestrator _shopOrchestrator;
        private readonly IBlobStorage _shopCarStorage;

        public CarsInShopOrchestrator(
            ICarOrchestrator carOrchestrator,
            IShopOrchestrator shopOrchestrator,
            IBlobStorage shopCarStorage)
        {
            _carOrchestrator = carOrchestrator;
            _shopOrchestrator = shopOrchestrator;
            _shopCarStorage = shopCarStorage;
        }

        public async Task<ShopCar> CreateAsync(Guid shopId, int carId)
        {
            if (shopId == Guid.Empty)
                throw new ArgumentException("Shop ID cannot be empty", nameof(shopId));

            if (carId <= 0)
                throw new ArgumentException("Car ID must be a positive integer", nameof(carId));

            var car = await _carOrchestrator.GetCarAsync(carId);
            if (car == null)
                throw new ArgumentException($"Car with ID {carId} not found", nameof(carId));

            var shop = await _shopOrchestrator.GetByIdAsync(shopId);
            if (shop == null)
                throw new ArgumentException($"Shop with ID {shopId} not found", nameof(shopId));

            var relationFileName = $"{shopId:N}_{carId}";

            if (!await _shopCarStorage.ContainsFileByNameAsync(relationFileName))
            {
                try
                {
                    await _shopCarStorage.PutContentAsync(relationFileName);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes 
                    //  _logger.LogError(ex, "Error storing shop-car relationship");

                    throw new InvalidOperationException("Failed to create the shop-car relationship.", ex);
                }
            }

            return new ShopCar
            {
                ShopId = shopId,
                CarId = carId
            };
        }

        public async Task<List<int>> GetCarsAsync(Guid shopId)
        {
            var shop = await _shopOrchestrator.GetByIdAsync(shopId);
            var carIds = await _shopCarStorage.FindByShopAsync(shopId);
            return carIds;
        }

        public async Task DeleteAsync(Guid shopId, int carId)
        {
            if (shopId == Guid.Empty)
               throw new ArgumentException("Shop ID cannot be empty", nameof(shopId));

            if (carId <= 0)
                throw new ArgumentException("Car ID must be a positive integer", nameof(carId));

            var relationFileName = $"{shopId:N}_{carId}";

            if (await _shopCarStorage.ContainsFileByNameAsync(relationFileName))
            {
                try
                {
                    await _shopCarStorage.DeleteAsync(relationFileName); 
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to delete the shop-car relationship.", ex);
                }
            }
        }
     }
 }