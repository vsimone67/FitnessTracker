using AutoMapper;
using FitnessTracker.Application.Workout.Events;
using FitnessTracker.Common.MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Workout.Command
{
    public class SaveDailyWorkoutToEventBusCommandHandler : IRequestHandler<SaveDailyWorkoutToEventBusCommand>
    {
        private readonly IServiceBus _serviceBus;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveDailyWorkoutToEventBusCommandHandler> _logger;
        public SaveDailyWorkoutToEventBusCommandHandler(IServiceBus eventBus, IMapper mapper, ILogger<SaveDailyWorkoutToEventBusCommandHandler> logger)
        {
            _serviceBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDailyWorkoutToEventBusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Save Daily Workout Event to Service Bus");
            var evt = new WorkoutCompletedEvent
            {
                CompletedWorkout = request.Workout
            };
            await _serviceBus.PublishMessage<WorkoutCompletedEvent>(evt);

            return await Task.FromResult(new Unit());
        }
    }
}