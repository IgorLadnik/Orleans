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
        bool Start();

        IEnumerable<Guid> Pieces { get; }
    }
}
