using Microsoft.AspNetCore.Mvc;
using SFood.BusinessInfo.Application;

namespace SFood.BusinessInfo.Host.Controllers
{
    public class RestaurantController : ControllerBase
    {
        private IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
    }
}
