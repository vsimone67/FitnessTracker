using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FitnessTracker.Common.Configuration;

namespace FitnessTracker.Service.Workout
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddAppConfigurationFromEnvironment()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}