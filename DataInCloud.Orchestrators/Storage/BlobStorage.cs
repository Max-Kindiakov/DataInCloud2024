using Azure.Storage.Blobs;
using DataInCloud.Model.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.Storage
{
    public class BlobStorage : IBlobStorage
    {
        private readonly string _containerName = "cars-in-shop";
        private readonly BlobServiceClient _client;
        public BlobStorage(BlobStorageConfig config)
        {
            _client = new BlobServiceClient(config.ConnectionString);
        }

        public async Task PutContentAsync(string fileName)
        {
            await _client.GetBlobContainerClient(_containerName).GetBlobClient(fileName).UploadAsync(new MemoryStream());
        }

        public async Task<bool> ContainsFileByNameAsync(string fileName)
        {
            return await _client.GetBlobContainerClient(_containerName).GetBlobClient(fileName).ExistsAsync();
        }

        public async Task<List<int>> FindByShopAsync(Guid ShopId)
        {
            var containerClient = _client.GetBlobContainerClient(_containerName);

            var blobPages = containerClient.GetBlobsAsync(prefix: ShopId.ToString("N")).AsPages(default, 1000);

            var results = new List<int>();

            await foreach (var page in blobPages)
            {
                var pageResults = page.Values.Select(bi => int.Parse(bi.Name.Split('_').Last()));
                results.AddRange(pageResults);
            }

            return results;
        }

        public async Task DeleteAsync(string fileName)
        {
            await _client.GetBlobContainerClient(_containerName).GetBlobClient(fileName).DeleteIfExistsAsync();
        }

    }


}
