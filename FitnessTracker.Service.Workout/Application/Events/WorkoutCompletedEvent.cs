using FitnessTracker.Application.Model.Workout;

namespace FitnessTracker.Application.Workout.Events
{
    public class WorkoutCompletedEvent
    {
        public DailyWorkoutDTO CompletedWorkout { get; set; }
    }
}