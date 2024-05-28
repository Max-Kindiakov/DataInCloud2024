using DataInCloud.Model.Shop;
using DataInCloud.Platform.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.Shop
{
    public class ShopOrchestrator : IShopOrchestrator
    {
        private readonly IShopRepository _repository;
        public IPublisher _publisher;

        public ShopOrchestrator(
            IShopRepository repository,
            IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<List<Model.Shop.Shop>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Model.Shop.Shop> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Model.Shop.Shop> CreateAsync(Model.Shop.Shop model)
        {
            var entity = await _repository.CreateAsync(model);
            await _publisher.PublishAsync(entity.Id);
            return entity;
        }

        public async Task<Model.Shop.Shop> UpdateAsync(Model.Shop.Shop modelToUpdate)
        {
            return await _repository.UpdateAsync(modelToUpdate);
        }

        public async Task<Model.Shop.Shop> DeleteAsync(Guid id)
        {
            return await (_repository.DeleteAsync(id));
        }
    }
}
