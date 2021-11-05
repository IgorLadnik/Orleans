using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using Data;
using Orleans.Streams;
using Infrastructure;

namespace Grains
{
    public class PieceGrain : Grain, IPieceGrain, IConsumerEventCountingGrain
    {
        //private int _numConsumedItems;
        //private ILogger _logger;
        private IAsyncObservable<IPieceEvent> _consumer; //stream
        private StreamSubscriptionHandle<IPieceEvent> _subscriptionHandle;
        //internal const string StreamNamespace = "HaloStreamingNamespace";
        private bool _isAlreadyConsumer = false;

        #region Implementation of IPieceGrain

        // GameId
        private Guid _gameId = Guid.Empty;
        public Task<Guid> GetGameId() => Task.Run(() => _gameId);
        public Task SetGameId(Guid gameId) => Task.Run(() => _gameId = gameId);

        // Rank
        private PieceRank _rank;
        public Task<PieceRank> GetRank() => Task.Run(() => _rank);
        public Task SetRank(PieceRank rank) => Task.Run(() => _rank = rank);

        // Color
        private PieceColor _color;
        public Task<PieceColor> GetColor() => Task.Run(() => _color);
        public Task SetColor(PieceColor color) => Task.Run(() => _color = color);

        // Location
        private PieceLocation _location;
        public Task<PieceLocation> GetLocation() => Task.Run(() => _location);
        public Task SetLocation(PieceLocation location) => Task.Run(() => _location = location);

        #endregion // Implementation of IPieceGrain

        #region OnActivateAsync & OnDeactivateAsync

        public override Task OnActivateAsync()
        {
            //_logger.Info("Consumer.OnActivateAsync");
            return base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            //_logger.Info("Consumer.OnDeactivateAsync");
            //await StopConsuming();
            await base.OnDeactivateAsync();
        }

        #endregion // OnActivateAsync & OnDeactivateAsync

        #region Implementation of IConsumerEventCountingGrain

        public async Task BecomeConsumer(Guid streamId, string providerToUse)
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
            _consumer = streamProvider.GetStream<IPieceEvent>(streamId, GrainIds.StreamNamespace);
            _subscriptionHandle = await _consumer.SubscribeAsync(new AsyncObserver<IPieceEvent>(EventArrived));

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

        //public Task<int> GetNumberConsumed()
        //{
        //    return Task.FromResult(_numConsumedItems);
        //}

        #endregion // Implementation of IConsumerEventCountingGrain

        private Task EventArrived(IPieceEvent @event)
        {
            //_numConsumedItems++;
            //_logger.Info("Consumer.EventArrived. NumConsumed so far: " + _numConsumedItems);
            return Task.CompletedTask;
        }
    }
}
