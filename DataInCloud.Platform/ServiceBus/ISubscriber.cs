using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Platform.ServiceBus
{
    public interface ISubscriber
    {
        //void Subscribe();
        Task SubscribeAsync();
        public List<string> Data { get; }
    }
}
