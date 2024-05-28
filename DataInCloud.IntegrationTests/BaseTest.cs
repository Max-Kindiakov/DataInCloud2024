using Autofac.Extensions.DependencyInjection;
using DataInCloud.Dal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace DataInCloud.IntegrationTests
{
    public class BaseTest : IDisposable
    {
        protected IHost Host;
        protected AppDbContext AppDbContext;
        protected CosmosDbContext CosmosDbContext;
        private IHostBuilder _server;
        public void Dispose()
        {
            StopServer();
        }


        public virtual HttpClient GetClient()
        {
            Host = _server.Start();
            AppDbContext = Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;
            CosmosDbContext = Host.Services.GetService(typeof(CosmosDbContext)) as CosmosDbContext;
            return Host.GetTestClient();
        }


        protected BaseTest InitTestServer()
        {
            _server = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseEnvironment("Development")
                .ConfigureWebHost(webhost =>
                {
                    webhost.UseTestServer();
                    webhost.UseStartup<TestStartup>();
                });
            return this;
        }


        private void StopServer()
        {
            Host?.StopAsync().GetAwaiter().GetResult();
        }
    }
}