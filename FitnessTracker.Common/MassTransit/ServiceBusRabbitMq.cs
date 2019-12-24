using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Threading.Tasks;

namespace FitnessTracker.Common.MassTransit
{
    public class ServiceBusRabbitMq : IServiceBus
    {
        protected IBusControl _serviceBus;
        protected ServiceBusSettings _serviceBusSettings;

        public ServiceBusRabbitMq(ServiceBusSettings serviceBusSettings)
        {
            _serviceBusSettings = serviceBusSettings;
        }

        public async Task Connect()
        {
            _serviceBus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri($"rabbitmq://{_serviceBusSettings.ServerName}:/"), h =>
                {
                    h.Username(_serviceBusSettings.UserName);
                    h.Password(_serviceBusSettings.Password);
                });

                ConfigureEndPoints(sbc, host, _serviceBusSettings.QueueName);
            });

            await _serviceBus.StartAsync();
        }

        public async Task PublishMessage<T>(T message)
        {
            await _serviceBus.Publish(message);
        }

        public async Task Disconnect()
        {
            await _serviceBus.StopAsync();
        }

        protected virtual void ConfigureEndPoints(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host, string queue)
        {
        }
    }
}