using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class RestaurantCategoryController : BaseController
    {
        private readonly IRestaurantCategoryService _restaurantCategoryService;
        private readonly IRepository _repository;

        public RestaurantCategoryController(IRestaurantCategoryService restaurantCategoryService,
            IRepository repository)
        {
            _restaurantCategoryService = restaurantCategoryService;
            _repository = repository;
        }

        /// <summary>
        /// 获取所有的商店分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ApiResponse> All()
        {
            var categories = await _restaurantCategoryService.GetAll();

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = categories
            };
        }
    }
}
