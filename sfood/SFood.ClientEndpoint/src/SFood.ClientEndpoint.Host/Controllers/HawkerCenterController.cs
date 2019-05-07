using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Host.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFood.ClientEndpoint.Host.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HawkerCenterController : ControllerBase
    {
        private readonly IHawkerCenterService _centerService;

        private readonly ILogger<HawkerCenterController> _logger;

        public HawkerCenterController(IHawkerCenterService centerService, ILogger<HawkerCenterController> logger)
        {
            _centerService = centerService;
            _logger = logger;
        }

        [HttpGet("restaurants")]
        public async Task<ApiResponse> GetRestaurants([FromQuery] GetRestaurantsParam param)
        {
            var restaurants = await _centerService.GetRestaurants(param);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = restaurants
            };
        }

        [HttpGet("rough")]
        public async Task<ApiResponse> GetCenterRough([FromQuery] GetCenterParam param)
        {
            var center = await _centerService.GetCenterRough(param);
            return new ApiResponse
            {
                StatusCode = Common.Enums.BusinessStatusCode.Success,
                Data = center
            };
        }
    }
}