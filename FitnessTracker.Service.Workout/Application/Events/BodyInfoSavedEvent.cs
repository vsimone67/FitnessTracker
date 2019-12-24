using FitnessTracker.Application.Model.Workout;

namespace FitnessTracker.Application.Workout.Events
{
    public class BodyInfoSavedEvent
    {
        public BodyInfoDTO SavedBodyInfo { get; set; }
    }
}