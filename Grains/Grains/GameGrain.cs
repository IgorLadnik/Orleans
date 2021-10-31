using System;
using System.Collections.Generic;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using System.Threading.Tasks;

namespace Grains
{
    public class GameGrain : Grain, IGameGrain
    {
        private IEnumerable<Guid> _pieceIds = new List<Guid>();

        public GameGrain()
        {
        }

        private int _testIntProp = 0;
        public Task<IEnumerable<Guid>> GetPieceIds() => Task.Run(() => _pieceIds);
        public Task SetPieceIds(IEnumerable<Guid> guids) => Task.Run(() => _pieceIds = guids);

        public Task<bool> Start()
        {
            return Task.Run(() => true);
        }
    }
}
