using System;
using Data;
using Orleans;

namespace GrainInterfaces
{
    public interface IPieceGrain : IGrainWithGuidKey
    {
        Guid GameId { get; set; }
        
        PieceColor Color { get; set; }
        PieceRank Rank { get; set; }
        PieceLocation Location { get; set; }

        bool Move(PieceLocation newLocation);
    }
}
