using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class StaffController : BaseController
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }
        
        [HttpPut("invitation"), Transactional]
        public async Task<ApiResponse> InviteStaff([FromBody] InviteStaffParam param)
        {
            
            param.RestaurantId = RestaurantId;            

            await _staffService.InviteStaff(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
        
        [HttpDelete, Transactional]
        public async Task<ApiResponse> DeleteStaff([FromBody] DeleteStaffParam param)
        {
            await _staffService.DeleteStaff(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
        
        [HttpPut, Transactional]
        public async Task<ApiResponse> ChangeStatus([FromBody] ChangeStatusParam param)
        {
            
            await _staffService.ChangeStaffStatus(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
        
        [HttpGet("all")]
        public async Task<ApiResponse> GetAll()
        {
            var staffs = await _staffService.GetAll(RestaurantId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = staffs
            };
        }

        [AllowAnonymous]
        [HttpPut("activation"), Transactional]
        public async Task<ApiResponse> Activate([FromBody] ActivateStaffParam param)
        {           
            await _staffService.ActivateStaff(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }
    }
}
