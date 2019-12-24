using AutoMapper;
using FitnessTracker.Common.MassTransit;
using FitnessTracker.Application.Workout.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Workout.Workout.Command
{
    public class SaveWorkoutToEventBusCommandHandler : IRequestHandler<SaveWorkoutToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveWorkoutToEventBusCommandHandler> _logger;

        public SaveWorkoutToEventBusCommandHandler(IServiceBus eventBus, IMapper mapper, ILogger<SaveWorkoutToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveWorkoutToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Save Workout Event to Service Bus");
            // write to event bus a new workout as been added
            var evt = new AddNewWorkoutEvent
            {
                AddedWorkout = request.Workout
            };

            await _serviceBus.PublishMessage<AddNewWorkoutEvent>(evt);

            return await Task.FromResult(new Unit());
        }
    }
}