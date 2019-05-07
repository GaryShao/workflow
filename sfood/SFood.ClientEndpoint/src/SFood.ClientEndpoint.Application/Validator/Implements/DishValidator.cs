using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.ClientEndpoint.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.Validator.Implements
{
    public class DishValidator : IDishValidator
    {
        private readonly IReadOnlyRepository _readonlyRepository;

        public DishValidator(IReadOnlyRepository readonlyRepository)
        {
            _readonlyRepository = readonlyRepository;
        }

        /// <summary>
        /// Validate if there dish id list contains any element that 
        /// doesn't belong to that restaurant at all
        /// </summary>
        /// <param name="dishIds"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task ValidateBadDishId(List<string> dishIds, string restaurantId)
        {
            var ids = (await _readonlyRepository.GetAllAsync<Dish>(d => 
                d.RestaurantId == restaurantId)).
                Select(d => d.Id).ToList();

            if (dishIds.Any(id => !ids.Contains(id)))
            {
                throw new BadRequestException($"The dish id list you gived contains bad element. ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dishIds"></param>
        /// <param name="restaurantId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task ValidateDuplicatedDishId(List<string> dishIds, string categoryId)
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
