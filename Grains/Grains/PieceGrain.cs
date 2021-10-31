﻿using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using GrainInterfaces;
using Data;

namespace Grains
{
    public class PieceGrain : Grain, IPieceGrain
    {
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
    }
}
