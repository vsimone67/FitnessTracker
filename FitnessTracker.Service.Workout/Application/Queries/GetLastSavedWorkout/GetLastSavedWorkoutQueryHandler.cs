using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Queries
{
    public class GetLastSavedWorkoutQueryHandler : HandlerBase<IWorkoutRepository, GetLastSavedWorkoutQueryHandler>, IRequestHandler<GetLastSavedWorkoutQuery, List<DailyWorkoutDTO>>
    {
        public GetLastSavedWorkoutQueryHandler(IWorkoutRepository repository, IMapper mapper, ILogger<GetLastSavedWorkoutQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<DailyWorkoutDTO>> Handle(GetLastSavedWorkoutQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetLastSavedWorkoutQueryHandler");

            var savedWorkout = await _repository.GetLastSavedWorkout(request.Id).ConfigureAwait(false);

            return _mapper.Map<List<DailyWorkoutDTO>>(savedWorkout);
        }
    }
}