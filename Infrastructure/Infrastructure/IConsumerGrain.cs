using System;
using System.Threading.Tasks;
using Orleans;

namespace Infrastructure
{
    public interface IConsumerGrain<TEvent> : IGrainWithGuidKey
    {
        Task BecomeConsumer(Guid streamId, string providerToUse, string streamNamespace, Func<TEvent, Task> processEvent = null);

        Task StopConsuming();

        Task<int> GetNumberConsumed();
    }
}
