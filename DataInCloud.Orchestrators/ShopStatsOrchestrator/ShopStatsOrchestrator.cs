using DataInCloud.Model.ShopStats;
using DataInCloud.Platform.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.ShopStatsOrchestrator
{
    public class ShopStatsOrchestrator : IShopStatsOrchestrator
    {
        private readonly ISubscriber _subscriber;

        public ShopStatsOrchestrator(ISubscriber subscriber)
        {
            _subscriber = subscriber;
        }
        public async Task<List<string>> GetStatsAsync()
        {
            return _subscriber.Data;
        }
    }
}
