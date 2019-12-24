using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Model.Diet.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Diet.Diet.Commands
{
    public class EditMetabolicInfoToEventBusCommandHandler : IRequestHandler<EditMetabolicInfoToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<EditMetabolicInfoToEventBusCommandHandler> _logger;
        public EditMetabolicInfoToEventBusCommandHandler(IServiceBus eventBus, ILogger<EditMetabolicInfoToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditMetabolicInfoToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Edit Metabolic Info Event to Service Bus");
            // write to event bus that the metabilic info has been edited
            var evt = new EditMetabolicInfo
            {
                EditedMetabolicInfo = request.MetabolicInfo
            };

            await _serviceBus.PublishMessage<EditMetabolicInfo>(evt);

            return await Task.FromResult(new Unit()).ConfigureAwait(false);
        }
    }
}