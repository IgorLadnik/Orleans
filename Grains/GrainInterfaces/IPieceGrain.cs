using System;
using System.Threading.Tasks;
using Orleans;
using Data;

namespace GrainInterfaces
{
    public interface IPieceGrain : IGrainWithGuidKey
    {
        // GameId
        Task<Guid> GetGameId();
        Task SetGameId(Guid gameId);

        // Rank
        Task<PieceRank> GetRank();
        Task SetRank(PieceRank rank);

        // Color
        Task<PieceColor> GetColor();
        Task SetColor(PieceColor color);

        // Location
        Task<PieceLocation> GetLocation();
        Task SetLocation(PieceLocation location);
    }
}
