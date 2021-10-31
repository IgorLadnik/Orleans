using System;
using System.Threading.Tasks;
using Data;
using Orleans;

namespace GrainInterfaces
{
    public interface IPieceGrain : IGrainWithGuidKey
    {
        //Task<Guid> GameId { get; set; }

        //Task<PieceColor> Color { get; set; }
        //Task<PieceRank> Rank { get; set; }
        //Task<PieceLocation> Location { get; set; }

        Task<bool> Move(PieceLocation newLocation);
    }
}
