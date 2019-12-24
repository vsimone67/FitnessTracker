using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Common.Metrics
{
    public class MetricsOptions
    {
        public bool Enabled { get; set; }
        public string InfluxDbUrl { get; set; }
        public string InfluxDbName { get; set; }
        public string ServerEnvironment { get; set; }
        public string ApplicationSuite { get; set; }
        public int HttpPolicyTimeOut { get; set; }
        public int FlushInterval { get; set; }
        public int ReportInterval { get; set; }
    }
}