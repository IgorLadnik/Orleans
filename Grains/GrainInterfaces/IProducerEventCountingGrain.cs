using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Data;

namespace GrainInterfaces
{
    public interface IProducerEventCountingGrain : IGrainWithGuidKey
    {
        Task BecomeProducer(Guid streamId, string providerToUse);

        /// <summary>
        /// Sends a single event and, upon successful completion, updates the number of events produced.
        /// </summary>
        /// <returns></returns>
        Task SendEvent(IPieceEvent @event);

        //Task<int> GetNumberProduced();
    }
}
