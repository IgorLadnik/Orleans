using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using GrainInterfaces;
using Grains;
using Data;

namespace WebAppOrleans1.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class GameController : Controller
    {
        private readonly Guid _testGuid = Guid.Parse("5B4DFADE-D577-4DA8-96FA-AA8AAA4BD0F2");
        private readonly Guid _gameGuid = Guid.Parse("778DA50A-B632-475C-8A3F-8D510310518E"); 

        private readonly IGrainFactory _grainFactory;

        public GameController(IGrainFactory grainFactory) => 
            _grainFactory = grainFactory;


        [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            var test = _grainFactory.GetGrain<ITestGrain>(_testGuid);
            await test.SetTestIntProp(7);
            var qa = await test.GetTestIntProp();

            var game = _grainFactory.GetGrain<IGameGrain>(_gameGuid);
            var br = await game.Start();
            var gameId = game.GetPrimaryKey();

            return Json($"{br} {qa}");
        }

        [HttpGet("{locations}")]
        public async Task<IActionResult> Move(string locations)
        {
            if (string.IsNullOrEmpty(locations))
                return Json(false);

            var ss = locations.Split('-');

            var game = _grainFactory.GetGrain<IGameGrain>(_gameGuid);
            var piece = await game.Move(new PieceLocation(ss[0]), new PieceLocation(ss[1]));
            return Json(piece != null 
                ? $"{await piece.GetRank()}, {await piece.GetColor()}, {await piece.GetLocation()}"
                : "Wrong move");
        }
    }
}
