using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using GrainInterfaces;

namespace WebAppOrleans1
{
    public class Program
    {
        public const string StoreName = "PubSubStore"; // Reserved name for streaming!
        public const string ProviderName = "SMSProvider";

        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans((ctx, siloBuilder) =>
                    siloBuilder
                        .UseLocalhostClustering()
                        .UseInMemoryReminderService()
                        .AddMemoryGrainStorage(StoreName/*, options => options.NumStorageGrains = 1*/)
                        .AddMemoryGrainStorageAsDefault()
                        .AddSimpleMessageStreamProvider(ProviderName)
                        .AddIncomingGrainCallFilter(async context =>
                        {
                            // If the method being called is 'Move', then set a value
                            // on the RequestContext which can then be read by other filters or the grain.
                            if (context.InterfaceMethod.Name == nameof(IGameGrain.Move))
                            {
                                //(context.Grain as Grain).GrainReference.GetPrimaryKey
                                RequestContext.Set("intercepted value", "this value was added by the filter");
                            }

                            await context.Invoke();

                            // If the grain method returned an int, set the result to double that value.
                            if (context.Result is IPieceGrain piece)
                                /*context.Result = */
                                await piece.SetLocation(new("e8")); // :)
                        })
                )
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
