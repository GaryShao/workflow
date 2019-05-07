using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Host.Controllers
{
    [AllowAnonymous]
    [ApiController, Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("menu")]
        public async Task<ApiResponse> GetCurrentMenu(string restaurantId)
        {
            var menu = await _restaurantService.GetCurrentMenuContent(restaurantId);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = menu
            };
        }

        [HttpGet("profile"), AllowAnonymous]
        public async Task<ApiResponse> GetProfile(string restaurantId)
        {
            var profile = await _restaurantService.GetRestaurantProfile(restaurantId);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = profile
            };
        }

        [HttpGet("dish/customizations")]
        public async Task<ApiResponse> GetCustomizations(string dishId)
        {
            var customizations = await _restaurantService.GetCustomizations(dishId);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = customizations
            };
        }

        [HttpGet("dishes")]
        public async Task<ApiResponse> GetDishes(string restaurantId, string searchWord=null)
        {
            var dishes = await _restaurantService.GetDishes(restaurantId, searchWord);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = dishes
            };
        }
    }
}
