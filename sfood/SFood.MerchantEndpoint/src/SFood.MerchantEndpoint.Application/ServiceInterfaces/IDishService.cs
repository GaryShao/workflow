using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using SFood.MerchantEndpoint.Application.Dtos.Results.Dish;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IDishService
    {
        Task<CreateDishResult> PostDish(CreateDishParam param);

        Task PutDish(EditDishParam param);

        Task DeleteDishes(DeleteDishParam param);

        Task<List<GetAllDishesResult>> GetAllDishes(string restaurantId);

        Task<DishResult> GetDish(string dishId);

        Task<List<DishBasicInfoResult>> GetAllUnassignedDishes(GetUnassignedDishesParam param);

        Task<List<DishBasicInfoResult>> GetAllDishesInCategory(string categoryId);

        Task UpdateDishIndex(UpdateDishIndexParam param);
    }
}
