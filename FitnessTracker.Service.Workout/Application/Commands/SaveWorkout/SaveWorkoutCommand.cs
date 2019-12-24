using FitnessTracker.Application.Model.Workout;
using MediatR;

namespace FitnessTracker.Application.Workout.Command
{
    public class SaveWorkoutCommand : IRequest<WorkoutDTO>
    {
        public WorkoutDTO Workout { get; set; }
    }
}