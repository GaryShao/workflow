using SFood.ClientEndpoint.Application.Dtos.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceInterfaces
{
    public interface IRestaurantService
    {
        Task<MenuContentResult> GetCurrentMenuContent(string restaurantId);

        Task<RestaurantProfileResult> GetRestaurantProfile(string restaurantId);

        Task<GetCustomizationsResult> GetCustomizations(string dishId);

        Task<List<MenuContentResult.DishDto>> GetDishes(string restaurantId, string searchWord);
    }
}
