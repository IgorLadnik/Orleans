using System.Threading.Tasks;
using Orleans;
using GrainInterfaces;

namespace Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        public Task<int> MuptiplyBy2(int a) => Task.Run(() => a * 2);

        private int _testIntProp = 0;
        public Task<int> GetTestIntProp() => Task.Run(() => _testIntProp);
        public Task SetTestIntProp(int n) => Task.Run(() => _testIntProp = n);
    }
}
