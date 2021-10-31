using System;
using System.Collections.Generic;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using System.Threading.Tasks;
using Data;
using System.Linq;

namespace Grains
{
    public class GameGrain : Grain, IGameGrain
    {
        private IList<Guid> _pieceIds = new List<Guid>();

        private IGrainFactory _grainFactory;

        public GameGrain(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

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

        private bool ValidateMove(IPieceGrain piece, PieceLocation to) => 
            piece != null;
    }
}
