using AutoMapper;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.MerchantEndpoint.Application.Dtos.Results.HawkerCenter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class HawkerCenterService : IHawkerCenterService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public HawkerCenterService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<List<HawkerCenterResult>> GetAll()
        {
            var centers = (await _readOnlyRepository.GetAllAsync<HawkerCenter>(hc => !hc.IsDeleted))
                .Select(hc => new HawkerCenterResult
                {
                    Id = hc.Id,
                    Name = hc.Name
                }).OrderBy(hc => hc.Name).ToList();
            return centers;
        }
    }
}
