using AutoMapper;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Common
{
    public class HandlerBase<TResult,TLogger>
    {
        protected IMapper _mapper;
        protected TResult _repository;
        protected ILogger<TLogger> _logger;
        public HandlerBase(TResult repository, IMapper mapper, ILogger<TLogger> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
    }
}