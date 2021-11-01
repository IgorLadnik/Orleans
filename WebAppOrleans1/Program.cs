using Data;
using GrainInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans((ctx, siloBuilder) =>
                {
                    siloBuilder.UseLocalhostClustering();

                    siloBuilder.AddIncomingGrainCallFilter(async context =>
                    {
                        // If the method being called is 'Move', then set a value
                        // on the RequestContext which can then be read by other filters or the grain.
                        if (context.InterfaceMethod.Name == nameof(IGameGrain.Move))
                        {
                            //(context.Grain as Grain).GrainReference.GetPrimaryKey
                            RequestContext.Set("intercepted value", "this value was added by the filter");
                        }

                        await context.Invoke();


                        //Task < IPieceGrain >

                        // If the grain method returned an int, set the result to double that value.
                        if (context.Result is IPieceGrain piece) 
                            /*context.Result = */await piece.SetLocation(new("Z8")); // :)
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}


//siloHostBuilder.AddIncomingGrainCallFilter(async context =>
//{
//    // If the method being called is 'MyInterceptedMethod', then set a value
//    // on the RequestContext which can then be read by other filters or the grain.
//    if (string.Equals(context.InterfaceMethod.Name, nameof(IMyGrain.MyInterceptedMethod)))
//    {
//        RequestContext.Set("intercepted value", "this value was added by the filter");
//    }

//    await context.Invoke();

//    // If the grain method returned an int, set the result to double that value.
//    if (context.Result is int resultValue) context.Result = resultValue * 2;
//});