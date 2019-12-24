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

namespace FitnessTracker.Application.Diet.Queries
{
    public class GetMetabolicInfoCalcTypeQueryHandler : HandlerBase<IDietRepository, GetMetabolicInfoCalcTypeQueryHandler>, IRequestHandler<GetMetabolicInfoCalcTypeQuery, CurrentMacrosDTO>
    {
        public GetMetabolicInfoCalcTypeQueryHandler(IDietRepository repository, IMapper mapper, ILogger<GetMetabolicInfoCalcTypeQueryHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<CurrentMacrosDTO> Handle(GetMetabolicInfoCalcTypeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetMetabolicInfoCalcTypeQueryHandler");

            List<MetabolicInfo> currentMacroList = await _repository.GetMetabolicInfoAsync();
            CurrentMacros currentMacro = new CurrentMacros();
            currentMacro.HydrateFromMetabolicInfo(currentMacroList, request.Id);

            return _mapper.Map<CurrentMacrosDTO>(currentMacro);
        }
    }
}