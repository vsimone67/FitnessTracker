using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Model.Diet.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Diet.Diet.Commands
{
    public class ProcessItemToEventBusCommandHandler : IRequestHandler<ProcessItemToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<ProcessItemToEventBusCommandHandler> _logger;

        public ProcessItemToEventBusCommandHandler(IServiceBus eventBus, ILogger<ProcessItemToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(ProcessItemToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Process Item To The Service Bus");
            var evt = new EditFoodItemEvent
            {
                EditedFoodItem = request.FoodInfo
            };

            await _serviceBus.PublishMessage<EditFoodItemEvent>(evt);

            return await Task.FromResult(new Unit()).ConfigureAwait(false);
        }
    }
}