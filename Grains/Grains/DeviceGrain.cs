using System;
using System.Threading.Tasks;
using Orleans.EventSourcing;
using GrainInterfaces;

namespace Grains
{
    public class DeviceGrain : JournaledGrain<DeviceState, DeviceEvent>, IDeviceGrain
    {
        //public DeviceGrain()
        //{
        //}

        public Task Act(int n) =>
            Task.Run(() => 
                RaiseEvent(new DeviceEvent { Prop = n, TimeStamp = DateTime.UtcNow }));
            
        public Task<int> GetState() => Task.Run(() => State.TheState);

        protected override void OnStateChanged()
        {
            // some thing ...
            base.OnStateChanged();
        }

        protected override void TransitionState(DeviceState state, DeviceEvent @event)
        {
            // code that updates the state

            base.TransitionState(state, @event);
        }
    }

    public class DeviceState
    {
        public int TheState { get; private set; } = 0;
        public DeviceState()
        { 
        }

        public void Apply(DeviceEvent @event)
        {
            // code that updates the state
            TheState = @event.Prop;
        }

        //public void Apply(E2 @event)
        //{
        //        // code that updates the state
        //}
    }

    public class DeviceEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; set; }
        public int Prop { get; set; }
    }
}
