﻿using FitnessTracker.Application.Common;
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
    public class GetWorkoutForDisplayQueryHandler : HandlerBase<IWorkoutRepository, GetWorkoutForDisplayQueryHandler>, IRequestHandler<GetWorkoutForDisplayQuery, WorkoutDisplayDTO>
    {
        // no automapper is needed so pass null
        public GetWorkoutForDisplayQueryHandler(IWorkoutRepository repository, ILogger<GetWorkoutForDisplayQueryHandler> logger) : base(repository, null, logger) { }

        public async Task<WorkoutDisplayDTO> Handle(GetWorkoutForDisplayQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetWorkoutForDisplayQueryHandler");

            FitnessTracker.Domain.Workout.Workout workout = await _repository.GetWorkoutForDisplayAsync(request.Id).ConfigureAwait(false);

            var lastSavedWorkout = await _repository.GetLastSavedWorkout(request.Id).ConfigureAwait(false);
            workout.DailyWorkout.Add(lastSavedWorkout);

            return AsyncHelper.RunSync<WorkoutDisplayDTO>(() => MakeWorkoutDTO(workout));
        }

        private Task<WorkoutDisplayDTO> MakeWorkoutDTO(Domain.Workout.Workout workout)
        {
            WorkoutDisplayDTO retval = new WorkoutDisplayDTO
            {
                WorkoutId = workout.WorkoutId,
                Name = workout.Name,
                Phase = string.Empty,
                isActive = workout.isActive,

                // No AutoMapper conversion here because there is special logic here (e.g. display repos and weight
                Set = workout.Set.OrderBy(set => set.SetOrder).Select(x => new SetDisplayDTO
                {
                    Name = x.SetName.Name,
                    SetId = x.SetId,
                    SetOrder = x.SetOrder,
                    WorkoutId = x.WorkoutId,
                    SetNameId = x.SetNameId,

                    DisplayReps = x.Exercise.First().Reps.OrderBy(ord => ord.RepsName.RepOrder).Select(rep => new RepsDisplayDTO
                    {
                        RepsId = rep.RepsId,
                        Name = rep.RepsName.Name,
                        RepOrder = rep.RepsName.RepOrder,
                        ExerciseId = rep.ExerciseId,
                        SetId = rep.SetId,
                        RepsNameId = rep.RepsNameId,
                    }).Distinct().ToList(),
                    Exercise = x.Exercise.OrderBy(exp => exp.ExerciseOrder).Select(ex => new ExerciseDisplayDTO
                    {
                        ExerciseId = ex.ExerciseId,
                        Name = ex.ExerciseName.Name,
                        ExerciseNameId = ex.ExerciseNameId,
                        ExerciseOrder = ex.ExerciseOrder,
                        Measure = ex.Measure,

                        SetId = x.SetId,
                        Reps = ex.Reps.OrderBy(ord => ord.RepsName.RepOrder).Select(rep => new RepsDisplayDTO
                        {
                            RepsId = rep.RepsId,
                            Weight = FindWeight(workout.DailyWorkout, x.SetId, ex.ExerciseId, rep.RepsId),
                            Name = rep.RepsName.Name,
                            TimeToNextExercise = rep.TimeToNextExercise,
                            RepOrder = rep.RepsName.RepOrder,
                            ExerciseId = rep.ExerciseId,
                            RepsNameId = rep.RepsNameId,
                            SetId = rep.SetId
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            CheckSetCount(retval);
            return Task.FromResult(retval);
        }

        private void CheckSetCount(WorkoutDisplayDTO workout)
        {
            int maxReps = GetMaxReps(workout);

            workout.Set.ForEach(set => { if (maxReps > set.DisplayReps.Count) set.AdditionalSets = maxReps - set.DisplayReps.Count; });
        }

        private int GetMaxReps(WorkoutDisplayDTO workout)
        {
            int maxReps = 0;

            workout.Set.ForEach(set => maxReps = Math.Max(set.DisplayReps.Count, maxReps));

            return maxReps;
        }

        protected int FindWeight(ICollection<DailyWorkout> dailyWorkout, int setId, int exerciseId, int repsId)
        {
            int retVal = 0;

            if (dailyWorkout.Any())
            {
                if (dailyWorkout.First() != null) // EF will return a row with null values
                {
                    var workout = dailyWorkout.First().DailyWorkoutInfo.Where(exp => exp.ExerciseId == exerciseId && exp.SetId == setId && exp.RepsId == repsId);

                    if (workout.Any())
                    {
                        retVal = workout.First().WeightUsed;
                    }
                }
            }
            return retVal;
        }
    }
}