using System;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using Data;

namespace Grains
{
    [StorageProvider(ProviderName = "games")]
    public class PieceGrain : Grain, IPieceGrain
    {
        public Guid GameId { get; set; }
        public PieceColor Color { get; set; }
        public PieceRank Rank { get; set; }
        public PieceLocation Location { get; set; }

        public bool Move(PieceLocation newLocation)
        {
            return true;
        }
    }
}
