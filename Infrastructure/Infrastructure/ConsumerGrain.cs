using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace Infrastructure
{
    public class ConsumerGrain<TEvent> : Grain, IConsumerGrain<TEvent>
    {
        private int _numConsumedItems;
        private IAsyncObservable<TEvent> _consumer; //stream
        private StreamSubscriptionHandle<TEvent> _subscriptionHandle;
        private bool _isAlreadyConsumer = false;

        public virtual async Task BecomeConsumer(Guid streamId, string providerToUse, string streamNamespace, Func<TEvent, Task> processEvent = null)
        {
            if (_isAlreadyConsumer)
                return;

            //_logger.Info("Consumer.BecomeConsumer");
            if (streamId == Guid.Empty)
            {
                throw new ArgumentNullException("streamId");
            }
            if (string.IsNullOrEmpty(providerToUse))
            {
                throw new ArgumentNullException("providerToUse");
            }

            var streamProvider = GetStreamProvider(providerToUse);
            _consumer = streamProvider.GetStream<TEvent>(streamId, streamNamespace);
            _subscriptionHandle = await _consumer.SubscribeAsync(new AsyncObserver<TEvent>(
                processEvent ?? 
                // default implementation of event processing
                (@event => Task.Run(() => 
                { 
                    _numConsumedItems++; 
                    return Task.CompletedTask; 
                }))
            ));

            _isAlreadyConsumer = true;
        }

        public async Task StopConsuming()
        {
            //_logger.Info("Consumer.StopConsuming");
            if (_subscriptionHandle != null && _consumer != null)
            {
                await _subscriptionHandle.UnsubscribeAsync();
                _subscriptionHandle = null;
                _consumer = null;
            }
        }

        public Task<int> GetNumberConsumed() =>
            Task.FromResult(_numConsumedItems);
    }
}
