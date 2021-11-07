using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace Infrastructure
{
    public class ProducerGrain<TEvent> : Grain, IProducerGrain<TEvent>
    {
        private IAsyncObserver<TEvent> _producer;
        private int _numProducedItems;
        private bool _isAlreadyProducer = false;

        public Task BecomeProducer(Guid streamId, string providerToUse, string streamNamespace)
        {
            if (_isAlreadyProducer)
                return Task.CompletedTask;

            //_logger.Info("Producer.BecomeProducer");
            if (streamId == Guid.Empty)
            {
                throw new ArgumentNullException("streamId");
            }
            if (string.IsNullOrEmpty(providerToUse))
            {
                throw new ArgumentNullException("providerToUse");
            }

            var provider = GetStreamProvider(providerToUse);
            _producer = provider.GetStream<TEvent>(streamId, streamNamespace);

            _isAlreadyProducer = true;
            return Task.CompletedTask;
        }

        public Task<int> GetNumberProduced() =>
            Task.FromResult(_numProducedItems);

        public async Task SendEvent(TEvent @event)
        {
            //_logger.Info("Producer.SendEvent called");
            if (_producer == null)
                throw new ApplicationException("Not yet a producer on a stream. Must call BecomeProducer first.");

            await _producer.OnNextAsync(@event);

            _numProducedItems++;
            //_logger.Info("Producer.SendEvent - TotalSent: ({0})", _numProducedItems);
        }
    }
}
