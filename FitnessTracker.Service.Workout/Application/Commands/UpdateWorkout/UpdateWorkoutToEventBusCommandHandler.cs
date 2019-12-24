using AutoMapper;
using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Model.Workout.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Workout.Workout.Command
{
    public class UpdateWorkoutToEventBusCommandHandler : IRequestHandler<UpdateWorkoutToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateWorkoutToEventBusCommandHandler> _logger;

        public UpdateWorkoutToEventBusCommandHandler(IServiceBus eventBus, IMapper mapper, ILogger<UpdateWorkoutToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateWorkoutToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Update Workout Event to Service Bus");

            // write to event bus a new workout as been added
            var evt = new EditWorkoutEvent
            {
                EditedWorkout = request.Workout
            };

            await _serviceBus.PublishMessage<EditWorkoutEvent>(evt);

            return await Task.FromResult(new Unit());
        }
    }
}