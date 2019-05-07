using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class CustomizationController : BaseController
    {
        private readonly ICustomizationService _customizationService;

        public CustomizationController(ICustomizationService customizationService)
        {
            _customizationService = customizationService;
        }

        [HttpPost("batch"), Transactional]
        public async Task<ApiResponse> SaveCustomizations([FromBody]SaveCustomizationParam param)
        {
            param.RestaurantId = RestaurantId;
            var result = new List<Application.Dtos.Results.Customization.CategoryResult>();

            if (param.IsNew)
            {
                result = await _customizationService.AddCustomizationCategoris(new AddCustomizationsParam {
                    RestaurantId = param.RestaurantId,
                    Categories = param.Categories
                });
            }
            else
            {
                result = await _customizationService.UpdateCustomizationCategories(new UpdateCustomizationsParam {
                    DishId = param.DishId,
                    RestaurantId = param.RestaurantId,
                    Categories = param.Categories
                });
            }
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = result
            };
        }

        [HttpGet("list")]
        public async Task<ApiResponse> GetCustomizations([FromQuery(Name = "dishesId")] string dishId)
        {
            var categories = await _customizationService.GetCustomizationCategories(dishId, RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = categories
            };
        }
    }
}
