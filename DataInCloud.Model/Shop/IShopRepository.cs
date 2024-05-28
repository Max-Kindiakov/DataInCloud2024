namespace DataInCloud.Model.Shop
{
    public interface IShopRepository
    {
        Task<Shop> CreateAsync(Shop model);
        Task<Shop> DeleteAsync(Guid id);
        Task<List<Shop>> GetAllAsync();
        Task<Shop> GetByIdAsync(Guid id);
        Task<Shop> UpdateAsync(Shop existingShop);
    }
}
