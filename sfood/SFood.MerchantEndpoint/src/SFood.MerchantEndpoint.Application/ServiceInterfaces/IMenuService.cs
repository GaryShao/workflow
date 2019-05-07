using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu;
using SFood.MerchantEndpoint.Application.Dtos.Results.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IMenuService
    {
        Task Add(CreateMenuParam param);

        Task Edit(EditRecipeParam param);

        Task Delete(string menuId);

        Task<MenuCategoryResult> AddDishCategory(CreateDishCategoryParam param);

        Task EditDishCategory(EditDishCategoryParam param);

        Task DeleteDishCategory(string dishCategoryId);

        Task ChangeDishOnShelfStatus(DishStatusInRecipeParam param);

        Task ChangeDishOnShelfStatusBatch(DishStatusBatchInRecipeParam param);

        Task<List<MenuRoughResult>> GetAll(string restaurantId);

        Task<AllMenuDetailResult> GetAllDetail(string restaurantId);

        Task<MenuDetailResult> GetDetail(string menuId);

        Task<List<RoughMenuInfoResult>> GetAllRoughMenuInfo(string restaurantId);

        Task Replicate(ReplicaParam param);
    }
}
