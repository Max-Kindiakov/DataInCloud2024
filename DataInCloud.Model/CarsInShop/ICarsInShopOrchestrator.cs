namespace DataInCloud.Model.CarsInShop
{
    public interface ICarsInShopOrchestrator
    {
        Task<ShopCar> CreateAsync(Guid shopId, int carId);
        Task<List<int>> GetCarsAsync(Guid shopId);
        Task DeleteAsync(Guid shopId, int carId);
    }
}