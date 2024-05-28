﻿using Microsoft.Extensions.Configuration;

namespace DataInCloud.Model.Storage
{
    public class BlobStorageConfig
    {
        private readonly IConfiguration _configuration;

        public BlobStorageConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ConnectionString => _configuration.GetConnectionString("BlobConnectionString");
    }
}
