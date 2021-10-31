using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using Orleans.Hosting;
using Grains;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans((ctx, siloBuilder) =>
                {
                    // In order to support multiple hosts forming a cluster, they must listen on different ports.
                    // Use the --InstanceId X option to launch subsequent hosts.
                    //var instanceId = ctx.Configuration.GetValue<int>("InstanceId");
                    var builder = siloBuilder.UseLocalhostClustering(
                        //siloPort: 11111 + instanceId,
                        //gatewayPort: 30000 + instanceId,
                        //primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, 11111)
                        );
                    builder.AddMemoryGrainStorage("games");

                    //?? builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GameGrain).Assembly));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.ConfigureKestrel((ctx, kestrelOptions) =>
                    //{
                    //    // To avoid port conflicts, each Web server must listen on a different port.
                    //    var instanceId = ctx.Configuration.GetValue<int>("InstanceId");
                    //    kestrelOptions.ListenLocalhost(5000 + instanceId);
                    //});
                });
    }
}
