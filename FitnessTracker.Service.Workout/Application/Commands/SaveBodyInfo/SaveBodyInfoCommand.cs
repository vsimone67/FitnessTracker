using FitnessTracker.Application.Model.Workout;
using MediatR;

namespace FitnessTracker.Application.Workout.Command
{
    public class SaveBodyInfoCommand : IRequest<BodyInfoDTO>
    {
        public BodyInfoDTO BodyInfo { get; set; }
    }
}