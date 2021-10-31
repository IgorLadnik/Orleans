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

        private int _testIntProp = 0;
        public Task<int> GetTestIntProp() => Task.Run(() => _testIntProp);
        public Task SetTestIntProp(int n) => Task.Run(() => _testIntProp = n);
    }

    public interface ITestGrain : IGrainWithGuidKey
    {
        Task<int> Ga(int a);
        Task<int> GetTestIntProp();
        Task SetTestIntProp(int n);
    }
}
