using System;
using System.Threading.Tasks;
using Orleans.Streams;

namespace Infrastructure
{
    public class AsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly Func<T, Task> _onNext;

        public AsyncObserver(Func<T, Task> onNext) =>
            _onNext = onNext;

        public Task OnNextAsync(T item, StreamSequenceToken token = null) =>
            _onNext(item);

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}
