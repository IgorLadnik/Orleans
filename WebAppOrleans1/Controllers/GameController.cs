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
        private readonly ConcreteGrainFactory cgf;

        public GameController(IGrainFactory grainFactory) =>
            cgf = new(grainFactory);

        [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            var test = cgf.Test;
            await test.SetTestIntProp(7);
            var qa = await test.GetTestIntProp();

            var br = await cgf.Game.Start();
            await cgf.Provider.BecomeProducer(GrainIds.StreamId, Program.ProviderName);

            // Device
            await cgf.Device.Act(11);

            return Json($"{br} {qa}");
        }

        [HttpGet("{locations}")]
        public async Task<IActionResult> Move(string locations)
        {
            if (string.IsNullOrEmpty(locations))
                return Json(false);

            // Device
            var deviceState = await cgf.Device.GetState();

            var ss = locations.Split('-');

            var piece = await cgf.Game.Move(new PieceLocation(ss[0]), new PieceLocation(ss[1]));
            await cgf.Consumer.BecomeConsumer(GrainIds.StreamId, Program.ProviderName);

            await cgf.Provider.SendEvent(new PieceEvent1 { Payload = "Sho?" });

            return Json(piece != null 
                ? $"{await piece.GetRank()}, {await piece.GetColor()}, {await piece.GetLocation()}"
                : "Wrong move");
        }
    }
}
