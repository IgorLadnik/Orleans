using Orleans;
using GrainInterfaces;
using Infrastructure;
using Data;

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

        public IProducerGrain<IPieceEvent> Provider =>
            _grainFactory.GetGrain<IProducerGrain<IPieceEvent>>(GrainIds.GameGrainId);

        public IConsumerGrain<IPieceEvent> Consumer =>
            _grainFactory.GetGrain<IConsumerGrain<IPieceEvent>>(GrainIds.GameGrainId);

    }
}
