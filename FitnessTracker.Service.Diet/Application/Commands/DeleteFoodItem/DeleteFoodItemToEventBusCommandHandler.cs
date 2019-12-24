using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Model.Diet.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Diet.Diet.Commands
{
    public class DeleteFoodItemToEventBusCommandHandler : IRequestHandler<DeleteFoodItemToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<DeleteFoodItemToEventBusCommandHandler> _logger;
        public DeleteFoodItemToEventBusCommandHandler(IServiceBus eventBus, ILogger<DeleteFoodItemToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteFoodItemToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Delete Food Item Event To The Service Bus");
            // write to event bus a new food item has been deleted
            var evt = new DeleteFoodItemEvent
            {
                DeletedFoodItem = request.FoodInfo
            };

            await _serviceBus.PublishMessage<DeleteFoodItemEvent>(evt);

            return await Task.FromResult(new Unit()).ConfigureAwait(false);
        }
    }
}