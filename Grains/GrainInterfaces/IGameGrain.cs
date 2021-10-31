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

        Task<IEnumerable<Guid>> GetPieceIds();
        Task SetPieceIds(IEnumerable<Guid> guids);
    }
}
