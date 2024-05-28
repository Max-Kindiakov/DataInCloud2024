using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace DataInCloud.Platform.ServiceBus
{
    public class ShopStatsPublisher : IPublisher
    {
        private readonly ServiceBusClient _client;
        protected virtual string QueueName => "inventory";
        private readonly ServiceBusSender _publisher;
        public ShopStatsPublisher(ServiceBusClient client)
        {
            _client = client;
            _publisher = _client.CreateSender(QueueName);
        }

        public async Task PublishAsync(Guid guid)
        {
            await _publisher.SendMessageAsync(new ServiceBusMessage(guid.ToString("N")));
        }
    }
}
