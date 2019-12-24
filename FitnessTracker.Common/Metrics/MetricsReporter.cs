using App.Metrics;
using App.Metrics.Scheduling;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Common.Metrics
{
    public class MetricsReporter : IHostedService
    {
        private readonly ILogger<MetricsReporter> _logger;
        private readonly IMetricsRoot _metrics;
        private readonly IOptions<MetricsOptions> _options;

        public MetricsReporter(ILogger<MetricsReporter> lgger, IMetricsRoot metrics, IOptions<MetricsOptions> options)
        {
            _logger = lgger;
            _metrics = metrics;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var requestSamplesScheduler = new AppMetricsTaskScheduler(TimeSpan.FromMilliseconds(_options.Value.ReportInterval), async () =>
            {
                await Task.WhenAll(_metrics.ReportRunner.RunAllAsync());  // send metrics to reporter
            });

            requestSamplesScheduler.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Done......");
            return Task.CompletedTask;
        }
    }
}