using FitnessTracker.Common.MassTransit;
using FitnessTracker.Common.Mvc;
using FitnessTracker.Common.SimpleInjector;
using FitnessTracker.Common.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using FitnessTracker.Common.HealthChecks;
using FitnessTracker.Common.Mediator;
using FitnessTracker.Common.AutoMapper;
using FitnessTracker.Common.DI;
using FitnessTracker.Common.Jaegar;
using FitnessTracker.Common.Logging;
using FitnessTracker.Common.Metrics;

namespace FitnessTracker.Service.Workout
{
    public class Startup
    {
        private readonly Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsPolicy()
                    .AddAppMetrics()
                    .AddCustomMvc()
                    .AddDiContainer(_container)
                    .AddSwaggerDocs()
                    .AddMassTransit()
                    .AddHealthCheck()
                    .AddCommandQueryHandlers()
                    .AddMappingProfiles()
                    .AddAutoRegisterDependencies()
                    .AddOpenTracing()
                    .AddJaeger()
                    .AddSeriLog();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDiContainer(_container)
               .UseAppMetrics()
               .UseCorsConfiguration()
               .UseCustomMvc()
               .UseSwaggerDocs()
               .UseMassTransit()
               .UseHealthCheck();
        }
    }
}