using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Queries
{
    public class GetExercisesQueryHandler : HandlerBase<IWorkoutRepository, GetExercisesQueryHandler>, IRequestHandler<GetExercisesQuery, List<ExerciseNameDTO>>
    {
        public GetExercisesQueryHandler(IWorkoutRepository repository, IMapper mapper, ILogger<GetExercisesQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<ExerciseNameDTO>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetExercisesQueryHandler");

            var exercises = await _repository.GetExercisesAsync().ConfigureAwait(false);

            return _mapper.Map<List<ExerciseNameDTO>>(exercises.OrderBy(exp => exp.Name));
        }
    }
}