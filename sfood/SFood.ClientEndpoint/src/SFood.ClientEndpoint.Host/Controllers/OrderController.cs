using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.Dtos.Parameters.Order;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Common.Enums;
using SFood.ClientEndpoint.Host.Attributes;
using SFood.ClientEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Host.Controllers
{    
    [ApiController, Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("place"), Transactional]
        public async Task<ApiResponse> Place([FromBody] PlaceOrderParam param)
        {
            param.UserId = UserId;
            var orderId = await _orderService.PlaceOrder(param);
            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = orderId
            };
        }

        [Authorize]
        [HttpGet("dealings")]
        public async Task<ApiResponse> GetAllDealing(int pageIndex = 0, int pageSize = 10)
        {
            var dealings = await _orderService.GetAllDealing(UserId, pageIndex, pageSize);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = dealings
            };
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ApiResponse> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            var all = await _orderService.GetAll(UserId, pageIndex, pageSize);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = all
            };
        }

        [AllowAnonymous]
        [HttpGet("detail")]
        public async Task<ApiResponse> GetDetail(string orderId)
        {
            var detail = await _orderService.GetDetail(orderId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = detail
            };
        }


        [HttpPut("cancel"), Transactional]
        public async Task<ApiResponse> Cancel([FromBody]CancelOrderParam param)
        {
            await _orderService.Cancel(param.OrderId, param.Reason, UserId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
    }
}
