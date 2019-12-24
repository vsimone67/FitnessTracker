using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Model.Diet.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Diet.Diet.Commands
{
    public class SaveMenuToEventBusCommandHandler : IRequestHandler<SaveMenuToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<SaveMenuToEventBusCommandHandler> _logger;

        public SaveMenuToEventBusCommandHandler(IServiceBus eventBus, ILogger<SaveMenuToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveMenuToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Save Menu To The Service Bus");
            // write to event bus that the menu has been saved
            var evt = new SaveMenuEvent
            {
                SavedMenu = request.Menu
            };

            await _serviceBus.PublishMessage<SaveMenuEvent>(evt);

            return await Task.FromResult(new Unit()).ConfigureAwait(false);
        }
    }
}