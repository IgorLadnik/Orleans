using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using GrainInterfaces;
using Grains;

namespace WebAppOrleans1.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class GameController : Controller
    {
        private readonly Guid _gameGuid = Guid.Parse("778DA50A-B632-475C-8A3F-8D510310518E"); 

        private readonly IGrainFactory _grainFactory;

        public GameController(IGrainFactory grainFactory) => 
            _grainFactory = grainFactory;


        [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            var test = _grainFactory.GetGrain<ITestGrain>(_gameGuid);
            var result = await test.Ga(5);

            //var game = _grainFactory.GetGrain<IGameGrain>(_gameGuid);


            //var gameIdTask = await player.CreateGame();
            //return Json(new { GameId = gameIdTask });
            //return Json(new { GameId = gameIdTask });
            return Json(result);
        }
    }
}
