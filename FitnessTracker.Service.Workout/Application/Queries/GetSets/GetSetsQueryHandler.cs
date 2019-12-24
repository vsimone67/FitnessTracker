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
    public class GetSetsQueryHandler : HandlerBase<IWorkoutRepository, GetSetsQueryHandler>, IRequestHandler<GetSetQuery, List<SetNameDTO>>
    {
        public GetSetsQueryHandler(IWorkoutRepository repository, IMapper mapper, ILogger<GetSetsQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<SetNameDTO>> Handle(GetSetQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetSetsQueryHandler");

            var setNames = await _repository.GetSetsAsync().ConfigureAwait(false);
            return _mapper.Map<List<SetNameDTO>>(setNames.OrderBy(exp => exp.Name));
        }
    }
}