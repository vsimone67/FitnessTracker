using AutoMapper;
using FitnessTracker.Application.Common;
using FitnessTracker.Application.Diet.Interfaces;
using FitnessTracker.Application.Model.Diet;
using FitnessTracker.Domain.Diet;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Diet.Command
{
    public class EditMetabolicInfoCommandHandler : HandlerBase<IDietRepository, EditMetabolicInfoCommandHandler>, IRequestHandler<EditMetabolicInfoCommand, MetabolicInfoDTO>
    {
        public EditMetabolicInfoCommandHandler(IDietRepository repository, IMapper mapper, ILogger<EditMetabolicInfoCommandHandler> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<MetabolicInfoDTO> Handle(EditMetabolicInfoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Edit Metabolic Info Command Handler");

            var metabolicInfo = _mapper.Map<MetabolicInfo>(request.MetabolicInfo);
            var editRecord = await _repository.EditMetabolicInfoAsync(metabolicInfo);

            return _mapper.Map<MetabolicInfoDTO>(editRecord);
        }
    }
}