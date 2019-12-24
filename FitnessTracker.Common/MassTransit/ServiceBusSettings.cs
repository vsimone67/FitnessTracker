namespace FitnessTracker.Common.MassTransit
{
    public class ServiceBusSettings
    {
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }

        public int NumberOfRetries { get; set; }
        public int RetryInterval { get; set; }

        public ServiceBusSettings()
        {
            NumberOfRetries = 3;
            RetryInterval = 5000;
        }
    }
}