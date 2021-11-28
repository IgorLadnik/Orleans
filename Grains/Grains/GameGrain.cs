using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using Data;
using Infrastructure;
using GrainInterfaces;

namespace Grains
{
    public class GameGrain : ProducerGrain<IPieceEvent>, IGameGrain
    {
        private IList<Guid> _pieceIds = new List<Guid>();

        private IGrainFactory _grainFactory;
        private IConfiguration _configuration;
        private ILogger<GameGrain> _logger;
        private IAsyncObserver<IPieceEvent> _producer;

        private int _numProducedItems;

        public GameGrain(IGrainFactory grainFactory, IConfiguration configuration, ILogger<GameGrain> logger) 
        {
            _grainFactory = grainFactory;
            _configuration = configuration;
            _logger = logger;
        }

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

                _logger.LogInformation($"***** {piece}, {await piece.GetRank()}");

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
