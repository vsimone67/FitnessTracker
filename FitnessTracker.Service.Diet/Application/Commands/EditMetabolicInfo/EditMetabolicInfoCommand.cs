using FitnessTracker.Application.Model.Diet;
using MediatR;

namespace FitnessTracker.Application.Diet.Command
{
    public class EditMetabolicInfoCommand : IRequest<MetabolicInfoDTO>
    {
        public MetabolicInfoDTO MetabolicInfo { get; set; }
    }
}