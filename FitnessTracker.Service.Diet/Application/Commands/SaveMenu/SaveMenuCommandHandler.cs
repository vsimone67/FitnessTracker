using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Diet.Interfaces;
using FitnessTracker.Application.Model.Diet;
using FitnessTracker.Domain.Diet;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Diet.Command
{
    public class SaveMenuCommandHandler : HandlerBase<IDietRepository, SaveMenuCommandHandler>, IRequestHandler<SaveMenuCommand, List<NutritionInfoDTO>>
    {
        public SaveMenuCommandHandler(IDietRepository repository, IMapper mapper, ILogger<SaveMenuCommandHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<NutritionInfoDTO>> Handle(SaveMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SaveMenuCommandHandler");

            await _repository.ClearSavedMenuAsync();

            var saveMenuCommandItem = _mapper.Map<List<NutritionInfo>>(request.Menu);

            foreach (var item in saveMenuCommandItem)
            {
                if (item.item.Count > 0)
                    await _repository.SaveMenuAsync(item);
            }           

            return request.Menu;
        }
    }
}