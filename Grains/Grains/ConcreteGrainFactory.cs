using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using GrainInterfaces;

namespace Grains
{
    public class ConcreteGrainFactory
    {
        private IGrainFactory _grainFactory;

        public ConcreteGrainFactory(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

        public ITestGrain Test =>
            _grainFactory.GetGrain<ITestGrain>(GrainIds.TestGrainId);

        public IDeviceGrain Device =>
            _grainFactory.GetGrain<IDeviceGrain>(GrainIds.DeviceGrainId);

        public IGameGrain Game =>
            _grainFactory.GetGrain<IGameGrain>(GrainIds.GameGrainId);

        public IProducerEventCountingGrain Provider =>
            _grainFactory.GetGrain<IProducerEventCountingGrain>(GrainIds.GameGrainId);

        public IConsumerEventCountingGrain Consumer =>
            _grainFactory.GetGrain<IConsumerEventCountingGrain>(GrainIds.GameGrainId);

    }
}
