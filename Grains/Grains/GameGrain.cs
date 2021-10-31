using System;
using System.Collections.Generic;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;

namespace Grains
{
    [StorageProvider(ProviderName = "games")]
    public class GameGrain : Grain, IGameGrain
    {
        public IEnumerable<Guid> Pieces { get; } = new List<Guid>();

        public GameGrain()
        {

        }

        public bool Start()
        {
            return true;
        }
    }
}
