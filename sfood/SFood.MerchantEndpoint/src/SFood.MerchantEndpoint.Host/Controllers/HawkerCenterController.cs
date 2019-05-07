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
    public class HawkerCenterController : BaseController
    {
        private readonly IHawkerCenterService _hawkerCenterService;
        private readonly IRepository _repository;

        public HawkerCenterController(IHawkerCenterService hawkerCenterService,
            IRepository repository)
        {
            _hawkerCenterService = hawkerCenterService;
            _repository = repository;
        }

        /// <summary>
        /// 获取所有Hawker Center的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ApiResponse> All()
        {
            var centers = await _hawkerCenterService.GetAll();

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = centers
            };
        }
    }
}
