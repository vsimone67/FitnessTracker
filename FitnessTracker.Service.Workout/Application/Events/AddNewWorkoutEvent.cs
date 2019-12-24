using FitnessTracker.Application.Model.Workout;

namespace FitnessTracker.Application.Workout.Events
{
    public class AddNewWorkoutEvent 
    {
        public WorkoutDTO AddedWorkout { get; set; }
    }
}