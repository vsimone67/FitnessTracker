using FitnessTracker.Common.HealthChecks;
using FitnessTracker.Common.Jaegar;
using FitnessTracker.Common.Metrics;
using FitnessTracker.Common.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace FitnessTracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppMetrics()
                   .AddCustomMvc()
                   .AddHealthCheck()
                   .AddOpenTracing()
                   .AddJaeger()
                   .AddOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAppMetrics()
                .UseCustomMvc()
                .UseHealthCheck()
                .UseCorsConfiguration();

            await app.UseOcelot();
        }
    }
}