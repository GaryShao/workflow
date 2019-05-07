using AutoMapper;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class RestaurantCategoryService : IRestaurantCategoryService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public RestaurantCategoryService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task<List<RestaurantCategoryResult>> GetAll()
        {
            var categories = (await _readOnlyRepository.GetAllAsync<RestaurantCategory>())
                .Select(rc => new RestaurantCategoryResult
                {
                    Id = rc.Id,
                    Name = rc.Name
                }).ToList();
            return categories;
        }
    }
}
