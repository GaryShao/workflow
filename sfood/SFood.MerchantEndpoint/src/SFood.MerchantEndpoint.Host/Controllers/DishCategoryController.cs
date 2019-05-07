using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class DishCategoryController : BaseController
    {
        private readonly IDishCategoryService _dishCategoryService;
        private readonly IRepository _repository;

        public DishCategoryController(IDishCategoryService dishCategoryService,
            IRepository repository)
        {
            _dishCategoryService = dishCategoryService;
            _repository = repository;
        }

        /// <summary>
        /// 向某个分类中添加若干菜品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("dishes"), Transactional]
        public async Task<ApiResponse> AddDishesToCategory([FromBody] AddDishesToCategoryParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _dishCategoryService.AddDishesToCategory(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 批量把若干菜品从其他分类中移动到某一分类下
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("dishes/transfer"), Transactional]
        public async Task<ApiResponse> TransferDishes([FromBody] TransferDishesParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _dishCategoryService.TransferDishes(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 从某个分类中删除若干菜品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("dishes"), Transactional]
        public async Task<ApiResponse> DeleteDishesFromCategory([FromBody] DeleteDishesFromMenuParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _dishCategoryService.DeleteDishesFromMenu(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpGet("all")]
        public async Task<ApiResponse> GetAllCategories([FromQuery] GetAllParam param)
        {            
            var categories = await _dishCategoryService.GetAllCategories(param);
            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = categories
            };
        }

        /// <summary>
        /// 更新菜单下的分类排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("sequence"), Transactional]
        public async Task<ApiResponse> UpdateIndexes(UpdateCategoryIndexesParam param)
        {                      
            await _dishCategoryService.UpdateCategoryIndexes(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
    }
}
