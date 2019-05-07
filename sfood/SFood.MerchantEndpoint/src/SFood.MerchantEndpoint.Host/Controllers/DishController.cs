using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class DishController : BaseController
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResponse> Get([FromQuery] GetDishParam param)
        {
            var dish = await _dishService.GetDish(param.DishesId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dish
            };
        }

        /// <summary>
        ///  添加菜品
        /// </summary>
        /// <param name="param"></param>
        [HttpPost, Transactional]
        public async Task<ApiResponse> AddDish([FromBody] CreateDishParam param)
        {
            param.RestaurantId = RestaurantId;
            var result = await _dishService.PostDish(param);

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = result
            };
        }

        [HttpPut, Transactional]
        public async Task<ApiResponse> EditDishBasicInfo([FromBody] EditDishParam param)
        {            
            await _dishService.PutDish(param);
            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 批量删除菜品 api
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete, Transactional]
        public async Task<ApiResponse> DeleteDish([FromBody] DeleteDishParam param)
        {            
            param.RestaurantId = RestaurantId;
            await _dishService.DeleteDishes(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpPut("sequence"), Transactional]
        public async Task<ApiResponse> UpdateIndexes([FromBody] UpdateDishIndexParam param)
        {
            await _dishService.UpdateDishIndex(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpGet("all")]
        public async Task<ApiResponse> GetAll()
        {
            var dishes = await _dishService.GetAllDishes(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dishes
            };
        }

        [HttpGet("category/all")]
        public async Task<ApiResponse> GetAllInCategory(string categoryId)
        {
            var dishes = await _dishService.GetAllDishesInCategory(categoryId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dishes
            };
        }       

        [HttpGet("unassigned")]
        public async Task<ApiResponse> GetAllUnassignedDishes([FromQuery] GetUnassignedDishesParam param)
        {            
            param.RestaurantId = RestaurantId;
            var dishes = await _dishService.GetAllUnassignedDishes(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dishes
            };
        }
    }
}
