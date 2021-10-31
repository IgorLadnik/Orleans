using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using Data;

namespace Grains
{
    public class PieceGrain : Grain, IPieceGrain
    {
        //private Guid _id;
        //public Task<Guid> GameId
        //{
        //    get => Task.Run(() => _id);
        //    set => Task.Run(() => _id = value.Result);
        //}

        //private PieceColor _color;
        //public Task<PieceColor> Color
        //{
        //    get => Task.Run(() => _color);
        //    set => Task.Run(() => _color = value.Result);
        //}

        //public PieceRank Rank { get; set; }
        //public PieceLocation Location { get; set; }

        public Task<bool> Move(PieceLocation newLocation)
        {
            return Task.Run(() => true);
        }
    }
}
