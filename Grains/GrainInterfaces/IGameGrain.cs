using Data;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IGameGrain : IGrainWithGuidKey
    {
        Task<bool> Start();

        Task<IList<Guid>> GetPieceIds();
        Task<IPieceGrain> Move(PieceLocation from, PieceLocation to);
    }
}
