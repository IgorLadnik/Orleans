using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface ITestGrain : IGrainWithGuidKey
    {
        Task<int> Ga(int a);
        Task<int> GetTestIntProp();
        Task SetTestIntProp(int n);
    }
}
