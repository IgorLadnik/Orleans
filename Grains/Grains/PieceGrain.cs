using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Infrastructure;
using GrainInterfaces;
using Data;

namespace Grains
{
    public class PieceGrain : ConsumerGrain<IPieceEvent>, IPieceGrain, IRemindable
    {
        //private ILogger _logger;

        #region Implementation of IPieceGrain

        // GameId
        private Guid _gameId = Guid.Empty;
        public Task<Guid> GetGameId() => Task.Run(() => _gameId);
        public Task SetGameId(Guid gameId) => Task.Run(() => _gameId = gameId);

        // Rank
        private PieceRank _rank;
        public Task<PieceRank> GetRank() => Task.Run(() => _rank);
        public Task SetRank(PieceRank rank) => Task.Run(() => _rank = rank);

        // Color
        private PieceColor _color;
        public Task<PieceColor> GetColor() => Task.Run(() => _color);
        public Task SetColor(PieceColor color) => Task.Run(() => _color = color);

        // Location
        private PieceLocation _location;
        public Task<PieceLocation> GetLocation() => Task.Run(() => _location);
        public Task SetLocation(PieceLocation location) => Task.Run(() => _location = location);

        #endregion // Implementation of IPieceGrain

        #region OnActivateAsync & OnDeactivateAsync

        public override Task OnActivateAsync()
        {
            //_logger.Info("Consumer.OnActivateAsync");
            
            // Reminder
            var grainReminder = RegisterOrUpdateReminder("game-reminder", TimeSpan.Zero, TimeSpan.FromMinutes(1)).Result;

            return base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            //_logger.Info("Consumer.OnDeactivateAsync");
            
            await StopConsuming();
            await base.OnDeactivateAsync();
        }

        #endregion // OnActivateAsync & OnDeactivateAsync

        public override async Task BecomeConsumer(Guid streamId, string providerToUse, string streamNamespace, Func<IPieceEvent, Task> processEvent) =>
            await base.BecomeConsumer(streamId, providerToUse, streamNamespace, processEvent ?? EventArrived);

        private Task EventArrived(IPieceEvent @event)
        {
            //_logger.Info("Consumer.EventArrived. NumConsumed so far: " + _numConsumedItems);
            return Task.CompletedTask;
        }

        // Reminder
        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            Console.WriteLine("Thanks for reminding me -- I almost forgot!");
            return Task.CompletedTask;
        }
    }
}
