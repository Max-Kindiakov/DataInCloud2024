namespace DataInCloud.Model.ShopStats
{
    public interface IShopStatsOrchestrator
    {
        Task<List<string>> GetStatsAsync();
    }
}
