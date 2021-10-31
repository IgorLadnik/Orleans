using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOrleans1.Controllers
{
    public class TestGrain : Grain, ITestGrain
    {
        public Task<int> Ga(int a) => Task.Run(() => a * 2);
    }

    public interface ITestGrain : IGrainWithGuidKey
    {
        Task<int> Ga(int a);
    }
}
