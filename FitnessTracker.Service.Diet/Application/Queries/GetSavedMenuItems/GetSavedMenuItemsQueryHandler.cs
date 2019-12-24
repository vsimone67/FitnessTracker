using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Diet.Interfaces;
using FitnessTracker.Application.Model.Diet;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Diet.Queries
{
    public class GetSavedMenuItemsQueryHandler : HandlerBase<IDietRepository, GetSavedMenuItemsQueryHandler>, IRequestHandler<GetSavedMenuItemsQuery, List<FoodInfoDTO>>
    {
        public GetSavedMenuItemsQueryHandler(IDietRepository repository, IMapper mapper, ILogger<GetSavedMenuItemsQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<List<FoodInfoDTO>> Handle(GetSavedMenuItemsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetSavedMenuItemsQueryHandler");

            var foodList = await _repository.GetAllFoodDataAsync();

            return _mapper.Map<List<FoodInfoDTO>>(foodList.OrderBy(exp => exp.Item).ToList());
        }
    }
}