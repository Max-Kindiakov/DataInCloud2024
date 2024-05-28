using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Platform.ServiceBus
{
    public class ShopStatsSubscriber : ISubscriber
    {
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        protected virtual string QueueName => "inventory";
        private readonly List<string> _receivedGuids = new List<string>();

        public ShopStatsSubscriber(ServiceBusClient client)
        {
            _client = client;
        }

        public List<string> Data => _receivedGuids;

        public async Task SubscribeAsync()
        {
            _processor = _client.CreateProcessor(QueueName);
            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += HandleErrorAsync;
            await _processor.StartProcessingAsync();
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            _receivedGuids.Add(args.Message.Body.ToString());
            await args.CompleteMessageAsync(args.Message);
        }

        private Task HandleErrorAsync(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error: {args.Exception.Message}");
            
            return Task.CompletedTask;
        }
    }
}