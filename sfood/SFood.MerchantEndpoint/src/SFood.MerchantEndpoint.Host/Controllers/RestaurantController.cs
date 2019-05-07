using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// 提交店铺基础资料
        /// </summary>
        /// <returns></returns>
        [HttpPost("basicinfo"), Transactional]
        public async Task<ApiResponse> UploadBasicInfo(RestaurantBasicInfoParam param)
        {            
            param.RestaurantId = RestaurantId;
            await _restaurantService.PostRestaurantBasicInfo(param);            

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 提交店铺审核资料
        /// </summary>
        /// <returns></returns>
        [HttpPost("qualificationinfo"), Transactional]
        public async Task<ApiResponse> UploadQualificationInfo(QualificationInfoParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantQualificationInfo(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 店铺设置提交
        /// </summary>
        /// <returns></returns>
        [HttpPut("config"), Transactional]
        public async Task<ApiResponse> EditConfiguration(RestaurantConfigurationParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantConfig(param);

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 获取店铺设置
        /// </summary>
        /// <returns></returns>
        [HttpGet("config")]
        public async Task<ApiResponse> GetConfiguration()
        {
            var config = await _restaurantService.GetRestaurantConfig(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = config
            };
        }

        /// <summary>
        /// 获取店铺的详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")] 
        public async Task<ApiResponse> GetProfile()
        {
            var profile = await _restaurantService.GetRestaurantProfile(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = profile
            };
        }

        /// <summary>
        /// 修改店铺的资料信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("detailedprofile"), Transactional]
        public async Task<ApiResponse> EditProfile([FromBody] RestaurantProfileParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantProfile(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改店铺的资料信息
        /// </summary>
        /// <param name="hello"></param>
        /// <returns></returns>
        [HttpPost("roughprofile"), Transactional]
        public async Task<ApiResponse> EditRoughProfile([FromBody] RestaurantRoughProfileParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantRoughProfile(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改店铺的图片信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("images"), Transactional]
        public async Task<ApiResponse> EditImages([FromBody] List<CategoriedImages> list)
        {
            
            var param = new RestaurantImagesParam { Categories = list };
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantImages(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改店铺的公告
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPost("announcement"), Transactional]
        public async Task<ApiResponse> EditAnnouncement([FromBody]
            RestaurantAnnouncementParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantAnnouncement(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改店铺的介绍
        /// </summary>
        /// <param name="introduction"></param>
        /// <returns></returns>
        [HttpPost("introduction"), Transactional]
        public async Task<ApiResponse> EditIntroduction([FromBody]
            RestaurantIntroductionParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.PostRestaurantIntroduction(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }        

        [HttpGet("qualification")]
        public async Task<ApiResponse> GetQualification()
        {
            var qualification = await _restaurantService.GetQualification(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = qualification
            };
        }

        [HttpPut("qualification"), Transactional]
        public async Task<ApiResponse> PostQualification([FromBody] UpdateQualificationParam param)
        {
            
            param.RestaurantId = RestaurantId;

            await _restaurantService.UpdateQualification(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpGet("index")]
        public async Task<ApiResponse> GetRoughInfo()
        {
            var index = await _restaurantService.GetRestaurantIndexInfo(RestaurantId);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = index
            };
        }

        [HttpPut("open"), Transactional]
        public async Task<ApiResponse> SwitchOpenOrClose(SwitchOpenOrCloseParam param)
        {
            
            param.RestaurantId = RestaurantId;
            await _restaurantService.SwitchToOpen(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpGet("open")]
        public async Task<ApiResponse> GetOpenStatus()
        {
            var isOpened = await _restaurantService.GetOpenStatus(RestaurantId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = isOpened
            };
        }

        [HttpGet("refusereasons")]
        public ApiResponse Get(byte isCanceled)
        {
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = new List<string> {
                    "The restaurant is too busy",
                    "The restaurant doesn't wanna take it"
                }                
            };
        }

    }
}
