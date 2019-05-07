using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.IdentityModels;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Account;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Host.Attributes;
using SFood.MerchantEndpoint.Host.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly ISmsService _smsService;
        private readonly IRepository _repository;

        public AccountController(
            IAccountService accountService,
            ISmsService smsService,
            IReadOnlyRepository readOnlyRepository,
            IRepository repository)
        {
            _accountService = accountService;
            _smsService = smsService;
            _readOnlyRepository = readOnlyRepository;
            _repository = repository;
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>        
        [HttpPut("password/retrieve"), Transactional, AllowAnonymous]
        public async Task<ApiResponse> RetrievePassword([FromBody] RetrievePasswordParam param)
        {           
            var countryCode = await _readOnlyRepository.GetFirstAsync<CountryCode>(cc => 
                cc.Id == param.CountryCodeId);

            if (countryCode == null)
            {
                throw new BadRequestException($"No such country code found in db. ");
            }

            var isValid = await _accountService.IsVerificationCodeValid(new VerificationCodeValidationParam {
                Phone = param.Phone,
                Code = param.Code,
                CountryCodeId = param.CountryCodeId
            });

            await _accountService.UpdatePasswordByPhone(param);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("password/reset"), Transactional]
        public async Task<ApiResponse> ResetPassword([FromBody] ResetPasswordParam param)
        {            
            param.UserId = UserId;

            var isOldPasswordValid = await _accountService.IsPassworRight(new VerificationPasswordParam
            {
                UserId = UserId,
                Password = param.OldPassword
            });

            if (!isOldPasswordValid)
            {
                return new ApiResponse
                {
                    StatusCode = BusinessStatusCode.Failed,
                    Message = "Wrong old password. "
                };
            }

            await _accountService.UpdatePasswordByUserId(param);

            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success                
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

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("vcode/validation"), AllowAnonymous]
        public async Task<ApiResponse> ValidVerificationCode([FromQuery] VerificationCodeValidationParam param)
        {
            var isValid = await _accountService.IsVerificationCodeValid(param);

            return new ApiResponse{
                StatusCode = BusinessStatusCode.Success,
                Data = isValid
            };
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <returns></returns>
        [HttpPut("phone"), Transactional]
        public async Task<ApiResponse> SetNewPhone([FromBody] ResetPhoneParam param)
        {
            

            var isValid = await _accountService.IsVerificationCodeValid(new VerificationCodeValidationParam
            {
                Phone = param.NewPhone,
                Code = param.NewCode
            });

            if (!isValid)
            {
                throw new BadRequestException("Code is not valid");
            }            

            await _accountService.UpdatePhone(new UpdatePhoneNumberParam
            {
                Phone = param.NewPhone,
                UserId = UserId
            });

            var countryCode = await _readOnlyRepository.GetFirstAsync<CountryCode>(cc => 
                cc.Id == param.CountryCodeId);

            if (countryCode == null)
            {
                throw new BadRequestException("Wrong country code. ");
            }

            var userExtension = await _readOnlyRepository.GetFirstAsync<UserExtension>(ue => 
                ue.UserId == UserId);

            userExtension.CountryCodeId = param.CountryCodeId;
            _repository.Update(userExtension);

            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success
            };
        }
        
        /// <summary>
        /// 获取开店引导
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/registration/guide")]        
        public ApiResponse GetGuide()
        {
            return new ApiResponse {
                StatusCode = BusinessStatusCode.Success,
                Data = new List<string>() {
                    "http://qiniu.sf-rush.com/ford_mustang_shelby_gt500-13.jpg",
                    "http://qiniu.sf-rush.com/thumb-1920-376894.jpg",
                    "http://qiniu.sf-rush.com/ford_mustang_shelby_gt500-5.jpg",
                    "http://qiniu.sf-rush.com/wallhaven-100855.jpg"
                },
                Message = "test"
            };
        }

        /// <summary>
        /// 获取注入协议
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/registration/protocal")]        
        public ApiResponse GetProtocal()
        {
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = @"<!DOCTYPE html><html><head><meta charset='utf-8'>
<title> 菜鸟教程(runoob.com) </title>
</head>
<body>

<h1> 我的第一个标题 </h1>
<p> 我的第一个段落。</p>

</body>
</html> ",
                Message = "test"
            };
        }

        /// <summary>
        /// 获取帮助文档
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/help")]
        public ApiResponse Helpers()
        {
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = new List<HelpResult>() {
                    new HelpResult{
                        Id = "4cef52a0-6a18-4773-8be9-272aecb5c55f",
                        Name = "教程1",
                        Url = "http://qiniu.sf-rush.com/ford_mustang_shelby_gt500-13.jpg"
                    },
                    new HelpResult{
                        Id = "ed450d7f-ebc3-4c3e-bd0d-16b91738a96d",
                        Name = "教程2",
                        Url = "http://qiniu.sf-rush.com/thumb-1920-376894.jpg"
                    },
                    new HelpResult{
                        Id = "9a72f2f0-1a7a-4dcc-84c8-caf30ccdef8c",
                        Name = "教程3",
                        Url = "http://qiniu.sf-rush.com/ford_mustang_shelby_gt500-5.jpg"
                    },
                    new HelpResult{
                        Id = "9ffa1f54-d0f7-4f9e-a1b7-d56ec2415717",
                        Name = "教程4",
                        Url = "http://qiniu.sf-rush.com/wallhaven-100855.jpg"
                    }
                }
            };
        }

        [HttpPut("logout"), Transactional]
        public async Task<ApiResponse> Logout()
        {
            await _accountService.Logout(UserId);
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success
            };
        }

        [HttpGet("countrycodes"), AllowAnonymous]
        public async Task<ApiResponse> GetCountryCodes()
        {
            var codes = await _accountService.GetDialingCodes();
            return new ApiResponse
            {
                StatusCode = BusinessStatusCode.Success,
                Data = codes
            };

        }
    }
}
