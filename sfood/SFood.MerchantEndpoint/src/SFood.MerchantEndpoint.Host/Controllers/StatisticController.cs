using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Statistic;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class StatisticController : BaseController
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        
        [HttpGet("info/timerange")]
        public async Task<ApiResponse> GetAll([FromQuery] GetInfoInTimeRangeParam param)
        {
            
            param.RestaurantId = RestaurantId;

            var list = await _statisticService.GetStatisticInfo(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = list
            };
        }

        [HttpGet("roughinfo/today")]
        public async Task<ApiResponse> GetRoughInfoOfToday()
        {
            var info = await _statisticService.TodayStatisticInfo(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = info
            };
        }

        [HttpGet("dishes")]
        public async Task<ApiResponse> GetDishStatisticInfo([FromQuery] DishStatisticInfoParam param)
        {
            
            param.RestaurantId = RestaurantId;            

            var dishes = await _statisticService.GetDishesStatistic(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dishes
            };
        }
        
        [AllowAnonymous]
        [HttpGet("income")]
        public async Task<ApiResponse> GetIncomeList([FromQuery] PagedIncomeListParam param)
        {
            
            param.RestaurantId = "16926122a55141c29278b77ec1965332";

            var income = await _statisticService.GetPagedIncomeList(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = income
            };
        }

    }
}
