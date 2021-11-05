using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using GrainInterfaces;
using Grains;
using Data;
using WebApplication1;

namespace WebAppOrleans1.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class GameController : Controller
    {
        private readonly IGrainFactory _grainFactory;

        public GameController(IGrainFactory grainFactory) => 
            _grainFactory = grainFactory;


        [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            var test = _grainFactory.GetGrain<ITestGrain>(GrainIds.TestGrainId);
            await test.SetTestIntProp(7);
            var qa = await test.GetTestIntProp();

            var br = await Game.Start();
            await Provider.BecomeProducer(GrainIds.StreamId, Program.ProviderName);

            // Device
            await Device.Act(11);

            return Json($"{br} {qa}");
        }

        [HttpGet("{locations}")]
        public async Task<IActionResult> Move(string locations)
        {
            if (string.IsNullOrEmpty(locations))
                return Json(false);

            // Device
            var deviceState = await Device.GetState();

            var ss = locations.Split('-');

            var piece = await Game.Move(new PieceLocation(ss[0]), new PieceLocation(ss[1]));
            await Consumer.BecomeConsumer(GrainIds.StreamId, Program.ProviderName);

            await Provider.SendEvent(new PieceEvent1 { Payload = "Sho?" });

            return Json(piece != null 
                ? $"{await piece.GetRank()}, {await piece.GetColor()}, {await piece.GetLocation()}"
                : "Wrong move");
        }

        private IDeviceGrain Device =>
            _grainFactory.GetGrain<IDeviceGrain>(GrainIds.DeviceGrainId);

        private IGameGrain Game =>
            _grainFactory.GetGrain<IGameGrain>(GrainIds.GameGrainId);

        private IProducerEventCountingGrain Provider =>
            _grainFactory.GetGrain<IProducerEventCountingGrain>(GrainIds.GameGrainId);

        private IConsumerEventCountingGrain Consumer =>
            _grainFactory.GetGrain<IConsumerEventCountingGrain>(GrainIds.GameGrainId);
    }
}
