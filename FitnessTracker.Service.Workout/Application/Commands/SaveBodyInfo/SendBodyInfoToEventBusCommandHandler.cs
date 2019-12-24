using FitnessTracker.Application.Workout.Events;
using FitnessTracker.Common.MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Workout.Command
{
    public class SendBodyInfoToEventBusCommandHandler : IRequestHandler<SendBodyInfoToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<SendBodyInfoToEventBusCommandHandler> _logger;

        public SendBodyInfoToEventBusCommandHandler(IServiceBus serviceBus, ILogger<SendBodyInfoToEventBusCommandHandler> logger)
        {
            _serviceBus = serviceBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendBodyInfoToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Send Body Info Event to Service Bus");
            // write to event bus that a bodyinfo was saved
            var evt = new BodyInfoSavedEvent
            {
                SavedBodyInfo = request.BodyInfo
            };

            await _serviceBus.PublishMessage<BodyInfoSavedEvent>(evt);

            return await Task.FromResult(new Unit());
        }
    }
}