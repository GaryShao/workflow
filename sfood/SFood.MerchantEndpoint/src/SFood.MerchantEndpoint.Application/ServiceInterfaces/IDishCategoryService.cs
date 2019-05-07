using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using SFood.MerchantEndpoint.Application.Dtos.Results.DishCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IDishCategoryService
    {
        Task AddDishesToCategory(AddDishesToCategoryParam param);

        Task<DishesInCategoryResult> GetAllDishes(string dishCategoryId);

        Task TransferDishes(TransferDishesParam param);

        Task DeleteDishesFromMenu(DeleteDishesFromMenuParam param);

        Task<List<DishCategoryResult>> GetAllCategories(GetAllParam param);

        Task UpdateCategoryIndexes(UpdateCategoryIndexesParam param);
    }
}
