namespace DataInCloud.Model.Storage
{
    public interface IBlobStorage
    {
        Task PutContentAsync(string fileName);
        Task<bool> ContainsFileByNameAsync(string fileName);
        Task<List<int>> FindByShopAsync(Guid shopId);
        Task DeleteAsync(string fileName);
    }
}