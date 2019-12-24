using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Model.Workout;
using FitnessTracker.Application.Workout.Interfaces;
using FitnessTracker.Common.Async;
using FitnessTracker.Domain.Workout;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Workout.Command
{
    public class SaveDailyWorkoutCommandHandler : HandlerBase<IWorkoutRepository, SaveDailyWorkoutCommandHandler>, IRequestHandler<SaveDailyWorkoutCommand, DailyWorkoutDTO>
    {
        public SaveDailyWorkoutCommandHandler(IWorkoutRepository repository, IMapper mapper, ILogger<SaveDailyWorkoutCommandHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<DailyWorkoutDTO> Handle(SaveDailyWorkoutCommand request, CancellationToken cancellationToken)
        {
            var dailyWorkout = AsyncHelper.RunSync<DailyWorkout>(() => MakeDailyWorkout(request.Workout));

            DailyWorkout savedWorkout = await _repository.SaveDailyWorkoutAsync(dailyWorkout).ConfigureAwait(false);

            return _mapper.Map<DailyWorkoutDTO>(savedWorkout);
        }

        private Task<DailyWorkout> MakeDailyWorkout(WorkoutDisplayDTO workout)
        {
            _logger.LogInformation("SaveDailyWorkoutCommandHandler");

            DailyWorkout dailyWorkout = new DailyWorkout();

            dailyWorkout.Phase = workout.Phase;
            dailyWorkout.WorkoutDate = DateTime.Now;
            dailyWorkout.WorkoutId = workout.WorkoutId;
            dailyWorkout.Duration = workout.Duration;

            foreach (SetDisplayDTO set in workout.Set)
            {
                set.Exercise.ForEach(ex => ex.Reps.ForEach(rep => dailyWorkout.DailyWorkoutInfo.Add(new DailyWorkoutInfo
                {
                    ExerciseId = ex.ExerciseId,
                    SetId = ex.SetId,
                    RepsId = rep.RepsId,
                    WeightUsed = rep.Weight,
                    WorkoutId = workout.WorkoutId
                })));
            }

            return Task.FromResult(dailyWorkout);
        }
    }
}