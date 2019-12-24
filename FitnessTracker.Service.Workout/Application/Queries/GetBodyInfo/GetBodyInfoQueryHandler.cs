using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Interfaces;
using FitnessTracker.Common.Async;
using FitnessTracker.Domain.Workout;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Queries
{
    public class GetBodyInfoQueryHandler : HandlerBase<IWorkoutRepository, GetBodyInfoQueryHandler>, IRequestHandler<GetBodyInfoQuery, List<BodyInfoDTO>>
    {
        public GetBodyInfoQueryHandler(IWorkoutRepository repository, IMapper mapper, ILogger<GetBodyInfoQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<BodyInfoDTO>> Handle(GetBodyInfoQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetBodyInfoQueryHandler");

            List<BodyInfo> bodyInfo = await _repository.GetBodyInfoAsync().ConfigureAwait(false);         

            // run this code in a separate thread so we do not block the main thread to allow better performance (this code will run sync on the new thread)
            AsyncHelper.RunSync(() => UpdateWeightParameters(bodyInfo));

            return _mapper.Map<List<BodyInfoDTO>>(bodyInfo);
        }

        private Task UpdateWeightParameters(List<BodyInfo> bodyInfo)
        {
            bodyInfo.OrderByDescending(info => info.Weight).First().isWorstWeight = true;
            bodyInfo.OrderByDescending(info => info.BodyFat).First().isWorstBodyFat = true;
            bodyInfo.OrderBy(info => info.Weight).First().isBestWeight = true;
            bodyInfo.OrderBy(info => info.BodyFat).First().isBestBodyFat = true;

            return Task.FromResult(1);
        }
    }

 
}