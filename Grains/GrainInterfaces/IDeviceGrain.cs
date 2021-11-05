using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task Act(int n);
        Task<int> GetState();
    }
}
