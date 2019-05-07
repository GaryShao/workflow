using Microsoft.AspNetCore.Mvc;
using SFood.BusinessInfo.Application;
using SFood.BusinessInfo.Host.Models;
using System;
using System.Threading.Tasks;

namespace SFood.BusinessInfo.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HawkerCenterController : ControllerBase
    {
        private readonly IHawkerCenterService _centerService;

        public HawkerCenterController(IHawkerCenterService centerService)
        {
            _centerService = centerService;
        }

        [HttpGet("restaurants")]
        public async Task<ApiResponse> GetRestaurants()
        {
            throw new NotImplementedException();
        }
    }
}
