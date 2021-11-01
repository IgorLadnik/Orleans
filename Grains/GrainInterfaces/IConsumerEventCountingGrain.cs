using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IConsumerEventCountingGrain : IGrainWithGuidKey
    {
        Task BecomeConsumer(Guid streamId, string providerToUse);

        Task StopConsuming();

        Task<int> GetNumberConsumed();
    }
}
