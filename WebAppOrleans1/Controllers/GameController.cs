using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Grains;
using Data;
using WebAppOrleans1;

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
            await cgf.Provider.BecomeProducer(GrainIds.StreamId, Program.ProviderName, GrainIds.StreamNamespace);

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
            await cgf.Consumer.BecomeConsumer(GrainIds.StreamId, Program.ProviderName, GrainIds.StreamNamespace);

            await cgf.Provider.SendEvent(new PieceEvent { Payload = $"new location: {ss[1]}" });

            return Json(piece != null 
                ? $"{await piece.GetRank()}, {await piece.GetColor()}, {await piece.GetLocation()}"
                : "Wrong move");
        }
    }
}
