using System;
using System.Threading.Tasks;
using Orleans;

namespace Infrastructure
{
    public interface IProducerGrain<TEvent> : IGrainWithGuidKey
    {
        Task BecomeProducer(Guid streamId, string providerToUse, string streamNamespace);

        Task SendEvent(TEvent @event);

        Task<int> GetNumberProduced();
    }
}
