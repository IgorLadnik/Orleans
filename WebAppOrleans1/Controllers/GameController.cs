using Microsoft.AspNetCore.Mvc;
using Orleans;
using Grains;
using Data;

// Usage:
// 1. https://localhost:5001/game/start
// 2. https://localhost:5001/game/e2-e4

namespace WebAppOrleans1.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class GameController : Controller
    {
        private readonly ConcreteGrainFactory _cgf;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Controller> _logger;

        public GameController(IGrainFactory grainFactory, IConfiguration configuration, ILogger<Controller> logger) 
        {
            _cgf = new(grainFactory);
            _configuration = configuration;
            _logger = logger;
        }

    [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            var test = _cgf.Test;
            await test.SetTestIntProp(7);
            var qa = await test.GetTestIntProp();

            var br = await _cgf.Game.Start();
            await _cgf.Provider.BecomeProducer(GrainIds.StreamId, Program.ProviderName, GrainIds.StreamNamespace);

            // Device
            await _cgf.Device.Act(11);

            return Json($"{br} {qa}");
        }

        [HttpGet("{locations}")]
        public async Task<IActionResult> Move(string locations)
        {
            if (string.IsNullOrEmpty(locations))
                return Json(false);

            // Device
            var deviceState = await _cgf.Device.GetState();

            var ss = locations.Split('-');

            var piece = await _cgf.Game.Move(new PieceLocation(ss[0]), new PieceLocation(ss[1]));
            await _cgf.Consumer.BecomeConsumer(GrainIds.StreamId, Program.ProviderName, GrainIds.StreamNamespace);

            await _cgf.Provider.SendEvent(new PieceEvent { Payload = $"new location: {ss[1]}" });

            return Json(piece != null 
                ? $"{await piece.GetRank()}, {await piece.GetColor()}, {await piece.GetLocation()}"
                : "Wrong move");
        }
    }
}
