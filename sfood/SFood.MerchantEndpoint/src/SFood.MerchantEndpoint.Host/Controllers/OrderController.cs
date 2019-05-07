using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.DataAccess.Common.Enums;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Order;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("detail")]
        public async Task<ApiResponse> GetDetail(string orderId)
        {
            var detail = await _orderService.GetDetail(orderId, RestaurantId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = detail
            };
        }
                
        [HttpGet("alloftoday")]
        public async Task<ApiResponse> GetTodayList(OrderStatus status, bool isPaged = false, int pageIndex = 0, int pageSize = 10)
        {
            var list = await _orderService.GetTodayList(status, isPaged, RestaurantId, pageIndex, pageSize);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = list
            };
        }

        [HttpGet("all")]
        public async Task<ApiResponse> GetAllList(int status, int pageIndex = 0, int pageSize = 10)
        {
            var list = await _orderService.GetAllList(status, RestaurantId, pageIndex, pageSize);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = list
            };
        }
        
        [HttpPut("status"), Transactional]
        public async Task<ApiResponse> ChangeStatus([FromBody] ChangeOrderStatusParam param)
        {
            
            param.RestaurantId = RestaurantId;

            if (param.ToStatus == OrderStatus.Closed && param.RefusedReason.IsNullOrEmpty())
            {
                throw new BadRequestException("You need to offer a reason to refuse an order. ");
            }

            await _orderService.ChangeOrderStatus(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
        
        [HttpGet("today/count")]
        public async Task<ApiResponse> GetTodayCount()
        {
            var count = await _orderService.GetTodayCount(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = count
            };
        }
    }
}
