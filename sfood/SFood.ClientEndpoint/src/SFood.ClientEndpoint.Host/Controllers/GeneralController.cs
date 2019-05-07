using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Common.Enums;
using SFood.ClientEndpoint.Host.Attributes;
using SFood.ClientEndpoint.Host.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Host.Controllers
{
    [AllowAnonymous]
    [ApiController, Route("api/[controller]")]
    public class GeneralController : BaseController
    {
        private readonly IGeneralService _generalService;
        private readonly ISmsService _smsService;

        public GeneralController(IGeneralService generalService
            , ISmsService smsService)
        {
            _generalService = generalService;
            _smsService = smsService;
        }

        [HttpGet("restaurantcategories")]
        public async Task<ApiResponse> GetRestaurantCategories()
        {
            var categories = await _generalService.GetRestaurantCategories();
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = categories
            };
        }

        [HttpGet("countrycodes")]
        public async Task<ApiResponse> GetCountryCodes()
        {
            var codes = await _generalService.GetCountryCodes();
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = codes
            };
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("vcode"), Transactional, AllowAnonymous]
        public async Task<ApiResponse> SendVerificationCode([FromBody]SendVCodeSMSParam param)
        {
            var elapsed = await _smsService.GetElapsedTimeOfLatestCode(param.Phone);

            if (elapsed.HasValue && elapsed < 60)
            {
                return new ApiResponse
                {
                    StatusCode = BusinessStatusCode.Success,
                    Data = 60 - Convert.ToInt32(elapsed.Value)
                };
            }

            await _smsService.SendVerficationCode(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = 60
            };
        }
    }
}
