using System.Threading.Tasks;

namespace FitnessTracker.Common.MassTransit
{
    public interface IServiceBus
    {
        Task Connect();
        Task Disconnect();
        Task PublishMessage<T>(T message);
    }
}