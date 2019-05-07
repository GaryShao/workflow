using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.Validator.Implements
{
    public class DishValidator : IDishValidator
    {
        private readonly IReadOnlyRepository _readonlyRepository;

        public DishValidator(IReadOnlyRepository readonlyRepository)
        {
            _readonlyRepository = readonlyRepository;
        }

        public async Task ValidateDishIds(List<string> dishIds, string restaurantId)
        {
            var ids = (await _readonlyRepository.GetAllAsync<Dish>(d => 
                d.RestaurantId == restaurantId)).
                Select(d => d.Id).ToList();

            if (dishIds.Any(id => !ids.Contains(id)))
            {
                throw new BadRequestException($"The dish id list you gived contains bad element. ");
            }
        }

        public async Task ValidateDuplicateIds(List<string> dishIds, string restaurantId, string categoryId)
        {
            var assignedIs = (await _readonlyRepository.GetAllAsync<Dish_DishCategory>(ddc =>
                ddc.DishCategoryId == categoryId)).
                Select(ddc => ddc.DishId).ToList();

            if (dishIds.Any(id => assignedIs.Contains(id)))
            {
                throw new BadRequestException($"The dish id list you gived contains id that already assigned to category");
            }
        }
    }
}
