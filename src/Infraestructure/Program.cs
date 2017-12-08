using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Preenactos.Infraestructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Preenactos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool runSeed = false;
            if (args.Contains("seed"))
            {
                runSeed = true;
                args = args.Where(d => d != "seed").ToArray();
            }
            var host = BuildWebHost(args);
            if (runSeed) RunSeed(host).Wait();
            else host.Run();
        }

        private static async Task RunSeed(IWebHost host)
        {
            Console.WriteLine("Running seed...");
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation("Seed services acquired");
                var context = services.GetService<Db>();
                logger.LogInformation("Context acquired");
                try
                {
                    await DbInitializer.Initialize(context, logger);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                    logger.LogError(ex, "An error occurred while seeding the database!!");
                }
                finally
                {
                    Console.WriteLine("Seed ended");
                    Console.ReadKey();
                }
            }
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}