using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using NSubstitute;
using HttpClientLib;
using GrainInterfaces;
using Data;

namespace Test1
{
    public class ServiceTests
    {
        const string ServiceUri = "https://localhost:7000/game/";

        private HttpClientWrapper _client;
        private IGameGrain  _gameGrainMock;
        private IPieceGrain _pieceGrainMock;

        [SetUp]
        public async Task Setup()
        {
            _client = new(new WebApplicationFactory<WebAppOrleans1.Startup>().CreateClient());

            // NSubstitute
            _gameGrainMock = Substitute.For<IGameGrain>();
            _pieceGrainMock = Substitute.For<IPieceGrain>();

            _pieceGrainMock.GetRank().Returns(Task.FromResult(PieceRank.Pawn));

            _gameGrainMock.Move(new("e2"), new("e4")).Returns(Task.FromResult(_pieceGrainMock));
        }

        [Test]
        public async Task Test_Start()
        {
            var query = "start";
            var content = await _client.GetAsync($"{ServiceUri}{query}");
            Assert.AreEqual("\"True 7\"", content);
        }

        [Test]
        public async Task Test_Move()
        {
            await Test_Start();

            var query = "e2-e4";
            var content = await _client.GetAsync($"{ServiceUri}{query}");

            Assert.IsTrue(content.Contains("White"));
            Assert.IsTrue(content.Contains("Pawn"));
        }


        [Test]
        public async Task Test_Move_Locally()
        {
            // NSubstitute
            var pieceGrainMock = await _gameGrainMock.Move(new("e2"), new("e4"));
            var rank = await pieceGrainMock.GetRank();
            Assert.AreEqual(PieceRank.Pawn, rank);
        }
    }
}