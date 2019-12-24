using FitnessTracker.Application.Model.Diet;
using MediatR;
using System.Collections.Generic;

namespace FitnessTracker.Application.Diet.Command
{
    public class SaveMenuCommand : IRequest<List<NutritionInfoDTO>>
    {
        public List<NutritionInfoDTO> Menu { get; set; }
    }
}