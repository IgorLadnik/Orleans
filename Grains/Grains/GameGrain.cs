using System;
using System.Collections.Generic;
using GrainInterfaces;
using System.Threading.Tasks;
using System.Linq;
using Orleans;
using Orleans.Streams;
using Orleans.Providers;
using Data;

namespace Grains
{
    public class GameGrain : Grain, IGameGrain, IProducerEventCountingGrain
    {
        private IList<Guid> _pieceIds = new List<Guid>();

        private IGrainFactory _grainFactory;

        //private IAsyncObserver<int> _producer;
        private IAsyncObserver<object> _producer;
        private int _numProducedItems;
        //private ILogger _logger;

        internal const string StreamNamespace = "HaloStreamingNamespace";

        public GameGrain(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

        #region Implementation of IGrainGrain

        public Task<IList<Guid>> GetPieceIds() => Task.Run(() => _pieceIds);

        public Task<bool> Start() =>
            Task.Run(async () => 
            {
                var piece = _grainFactory.GetGrain<IPieceGrain>(Guid.NewGuid());
                await piece.SetGameId(this.GetPrimaryKey());
                await piece.SetRank(PieceRank.Pawn);
                await piece.SetColor(PieceColor.White);
                await piece.SetLocation(new PieceLocation("e2"));
                _pieceIds.Add(piece.GetPrimaryKey());

                return piece != null;       
            });

        public Task<IPieceGrain> Move(PieceLocation from, PieceLocation to) =>
            Task.Run(async () => 
            {
                var piece = await _pieceIds.Select(async id =>
                {
                    var piece = _grainFactory.GetGrain<IPieceGrain>(id);
                    return await piece.GetLocation() == from ? piece : null;
                }).Where(p => p != null).FirstOrDefault();

                var br = ValidateMove(piece, to);
                if (br)
                    await piece.SetLocation(to);

                return br ? piece : null;       
            });

        #endregion // Implementation of IGrainGrain

        #region Implementation of IProducerEventCountingGrain

        public Task BecomeProducer(Guid streamId, string providerToUse)
        {
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
            _producer = provider.GetStream<object>(streamId, StreamNamespace);
            return Task.CompletedTask;
        }

        public Task<int> GetNumberProduced()
        {
            return Task.FromResult(_numProducedItems);
        }

        public async Task SendEvent(object ob)
        {
            //_logger.Info("Producer.SendEvent called");
            if (_producer == null)
            {
                throw new ApplicationException("Not yet a producer on a stream.  Must call BecomeProducer first.");
            }

            //await _producer.OnNextAsync(_numProducedItems + 1);
            await _producer.OnNextAsync(ob);

            // update after send in case of error
            _numProducedItems++;
            //_logger.Info("Producer.SendEvent - TotalSent: ({0})", _numProducedItems);
        }

        #endregion // Implementation of IProducerEventCountingGrain

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

        private bool ValidateMove(IPieceGrain piece, PieceLocation to) => 
            piece != null;
    }
}
