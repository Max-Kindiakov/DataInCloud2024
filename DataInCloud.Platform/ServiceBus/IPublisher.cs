using System;
using System.Threading.Tasks;

namespace DataInCloud.Platform.ServiceBus
{
    public interface IPublisher
    {
        Task PublishAsync(Guid guid);
    }
}
