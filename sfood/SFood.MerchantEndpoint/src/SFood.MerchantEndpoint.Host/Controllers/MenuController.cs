using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IRepository _repository;

        public MenuController(IMenuService menuService,
            IRepository repository)
        {
            _menuService = menuService;
            _repository = repository;
        }

        /// <summary>
        /// 添加菜单接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Transactional]
        public async Task<ApiResponse> Add([FromBody] CreateMenuParam param)
        {
            param.RestaurantId = RestaurantId;

            await _menuService.Add(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改菜单接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut, Transactional]
        public async Task<ApiResponse> Edit([FromBody] EditRecipeParam param)
        {            
            param.RestaurantId = RestaurantId;

            await _menuService.Edit(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 删除菜单接口
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpDelete, Transactional]
        public async Task<ApiResponse> Delete([FromBody] DeleteMenuParam param)
        {            
            await _menuService.Delete(param.MenuId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 在菜单中添加分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("dishcategory"), Transactional]
        public async Task<ApiResponse> AddDishCategory([FromBody] CreateDishCategoryParam param)
        {            
            param.RestaurantId = RestaurantId;

            await _menuService.AddDishCategory(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 在菜单中修改分类
        /// </summary>
        /// <returns></returns>
        [HttpPut("dishcategory"), Transactional]
        public async Task<ApiResponse> EditDishCategory([FromBody] EditDishCategoryParam param)
        {
            

            await _menuService.EditDishCategory(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 删除菜单分类接口
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpDelete("dishcategory"), Transactional]
        public async Task<ApiResponse> DeleteDishCategory([FromBody] DeleteDishCategoryParam param)
        {
            

            await _menuService.DeleteDishCategory(param.CategoryId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 上下架菜品接口
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPut("dishstatus"), Transactional]
        public async Task<ApiResponse> ChangeOnShelfBatch(DishStatusBatchInRecipeParam param)
        {
            

            await _menuService.ChangeDishOnShelfStatusBatch(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ApiResponse> GetMenuDetail(string menuId)
        {
            var menuDetail = await _menuService.GetDetail(menuId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = menuDetail
            };
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ApiResponse> All()
        {
            var menus = await _menuService.GetAll(RestaurantId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = menus
            };
        }

        /// <summary>
        /// 获取菜单详情列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail/all")]
        [AllowAnonymous]
        public async Task<ApiResponse> GetAllMenuDetail()
        {
            var restaurantId = "1ed20f10cd8c4bc3ab1d3b2f202b96c5";
            var menuDetails = await _menuService.GetAllDetail(restaurantId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = menuDetails
            };
        }

        /// <summary>
        /// 获取菜单概略的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("rough/all")]
        public async Task<ApiResponse> GetAllMenuRough()
        {
            var roughList = await _menuService.GetAllRoughMenuInfo(RestaurantId);

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = roughList
            };
        }

        /// <summary>
        /// 获取菜单概略的列表
        /// </summary>
        /// <returns></returns>
        [HttpPut("replica"), Transactional]
        public async Task<ApiResponse> Replica([FromBody] ReplicaParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _menuService.Replicate(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
    }
}
