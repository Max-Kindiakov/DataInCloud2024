namespace DataInCloud.Model.Shop
{
    public interface IShopOrchestrator
    {
        Task<List<Shop>> GetAllAsync();
        Task<Shop> GetByIdAsync(Guid id);
        Task<Shop> CreateAsync(Shop model);
        Task<Shop> UpdateAsync(Shop modelToUpdate);
        Task<Shop> DeleteAsync(Guid id);
    }
}
