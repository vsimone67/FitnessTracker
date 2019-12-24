using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Workout.Command
{
    public class UpdateWorkoutCommandHandler : HandlerBase<IWorkoutRepository, UpdateWorkoutCommandHandler>, IRequestHandler<UpdateWorkoutCommand, WorkoutDTO>
    {
        public UpdateWorkoutCommandHandler(IWorkoutRepository repository, IMapper mapper, ILogger<UpdateWorkoutCommandHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<WorkoutDTO> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateWorkoutCommandHandler");

            var workoutDTO = _mapper.Map<FitnessTracker.Domain.Workout.Workout>(request.Workout);
            var workout = await _repository.UpdateWorkoutAsync(workoutDTO);
            return _mapper.Map<WorkoutDTO>(workout);
        }
    }
}