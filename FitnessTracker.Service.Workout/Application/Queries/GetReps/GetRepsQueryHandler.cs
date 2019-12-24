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
    public class GetRepsQueryHanlder : HandlerBase<IWorkoutRepository, GetRepsQueryHanlder>, IRequestHandler<GetRepsQuery, List<RepsNameDTO>>
    {
        public GetRepsQueryHanlder(IWorkoutRepository repository, IMapper mapper, ILogger<GetRepsQueryHanlder> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<RepsNameDTO>> Handle(GetRepsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetRepsQueryHanlder");

            var reps = await _repository.GetRepsAsync().ConfigureAwait(false);

            return _mapper.Map<List<RepsNameDTO>>(reps.OrderBy(exp => exp.RepOrder));
        }
    }
}