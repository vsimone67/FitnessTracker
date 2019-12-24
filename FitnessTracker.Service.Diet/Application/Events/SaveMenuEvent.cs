using System.Collections.Generic;

namespace FitnessTracker.Application.Model.Diet.Events
{
    public class SaveMenuEvent
    {
        public List<NutritionInfoDTO> SavedMenu { get; set; }
    }
}