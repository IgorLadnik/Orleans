using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using HttpClientLib;

namespace Test1
{
    public class ServiceTests
    {
        const string ServiceUri = "https://localhost:7000/game/";

        private HttpClientWrapper _client;

        [SetUp]
        public void Setup() =>
            _client = new(new WebApplicationFactory<WebAppOrleans1.Startup>().CreateClient());

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
    }
}