﻿using AutoMapper;
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
    public class GetAllWorkoutsQueryHandler : HandlerBase<IWorkoutRepository, GetAllWorkoutsQueryHandler>, IRequestHandler<GetAllWorkoutsQuery, List<WorkoutDTO>>
    {
        public GetAllWorkoutsQueryHandler(IWorkoutRepository repository, IMapper mapper, ILogger<GetAllWorkoutsQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<WorkoutDTO>> Handle(GetAllWorkoutsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllWorkoutsQueryHandler");

            var workouts = await _repository.GetAllWorkoutsAsync().ConfigureAwait(false);

            List<WorkoutDTO> retval;

            if (request.IsActive)
                retval = _mapper.Map<List<WorkoutDTO>>(workouts.Where(exp => exp.isActive).OrderBy(exp => exp.Name)); // only return active
            else
                retval = _mapper.Map<List<WorkoutDTO>>(workouts);  // return all

            return retval;
        }
    }
}