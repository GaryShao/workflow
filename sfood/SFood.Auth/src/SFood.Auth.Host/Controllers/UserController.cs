using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFood.Auth.Common.Consts;
using SFood.Auth.Common.Enums;
using SFood.Auth.Common.Extensions;
using SFood.Auth.Host.Exceptions;
using SFood.Auth.Host.Models;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFood.Auth.Host.Controllers
{
    [ApiController, Route("api/owner")]
    public class UserController: ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager,
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IReadOnlyRepository readOnlyRepository,
            IRepository repository,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _repository = repository;            
            _readOnlyRepository = readOnlyRepository;
            _configuration = configuration;
            _httpClient = clientFactory.CreateClient("FetchToken");
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("client")]
        public async Task<ApiResponse> ClientAccess([FromBody] ClientAccessModel param)
        {
            var user = await _readOnlyRepository.GetFirstAsync<User>(u =>
                u.PhoneNumber == param.Phone);

            if (user == null)
            {
                //register new one
                return await RegisterAsClient(new RegisterParam {
                    Name = param.Phone,
                    Password = "123456sf",
                    Email = param.Phone + "@sfmail.com",
                    VerificationCode = param.VCode,
                    Phone = param.Phone,
                    CountryCodeId = param.CountryCodeId,
                    AgreePolicy = true
                });
            }
            else
            {
                string roleId = null;
                //fact: the user already exist, but we not sure it's a client or restaurant roles
                using (var dbContext = new DapperDbContext(_configuration))
                {
                    var sql = @"SELECT [RoleId]
                                FROM [IdentitySchema].[UserRoles]
                                WHERE UserId = @userId;";

                    var retrieveParam = new DynamicParameters();

                    retrieveParam.Add("userId", user.Id);

                    roleId = await dbContext.Connection.QueryFirstAsync<string>(sql, retrieveParam);
                }

                var role = await _readOnlyRepository.GetFirstAsync<Role>(r 
                    => r.Id == roleId);

                if (role.Name != AppConst.Client)
                {
                    throw new BadRequestException("The phone has been registered as restaurant owner or employee");
                }

                //just get the token of that user

                return await TokenAsClient(new FetchTokenParam
                {
                    ClientId = "clientH5",
                    ClientSecret = "secret",
                    GrantType = "password",
                    Phone = param.Phone,
                    CountryCodeId = param.CountryCodeId,
                    Password = "123456sf",
                    VCode = param.VCode
                });
            }
        }


        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ApiResponse> Register([FromBody] RegisterParam param)
        {
            if (!param.AgreePolicy)
            {
                throw new BadRequestException("You need to agree to our policy at first.");
            }

            if (!ModelState.IsValid)
            {
                var error = ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .Select(e => e.First().ErrorMessage)
                .First();
                throw new BadRequestException(error);
            }

            if (IsEmailExist(param.Email))
            {
                throw new BadRequestException("The email has already existed in our db");
            }

            if (IsPhoneExist(param.Phone))
            {
                throw new BadRequestException("The phone has already existed in our db");
            }

            //var verificationCode = await _readOnlyRepository.GetFirstAsync<VerificationCode>(c => c.Phone == param.Phone &&
            //                (DateTime.UtcNow - c.CreatedTime).TotalMinutes < 5,
            //                codes => codes.OrderByDescending(c => c.CreatedTime));

            //if (verificationCode == null)
            //{
            //    throw new Exception($"Invalid verification code. ");
            //}

            //if (verificationCode.Code != param.VerificationCode)
            //{
            //    throw new BadRequestException("Invalid verification code. ");
            //}

            var userId = Guid.NewGuid().ToUuidString();

            var result = await _userManager.CreateAsync(new User {
                Id = userId,
                UserName = param.Phone,
                PhoneNumber = param.Phone,
                Email = param.Email,
                PasswordHash = param.Password
            });

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(string.Join(",", errors));
            }            
           
            var user = _readOnlyRepository.GetOne<User>(u => u.Id == userId);
            var role = _readOnlyRepository.GetOne<Role>(r => r.Name == AppConst.RestaurantOwner);

            using (var dbContext = new DapperDbContext(_configuration))
            {
                var sql = @"INSERT INTO [IdentitySchema].[UserRoles]
                            ([UserId] ,[RoleId])
                            VALUES
                            (@userId, @roleId);";

                var addRoleParams = new DynamicParameters();

                addRoleParams.Add("userId", userId);
                addRoleParams.Add("roleId", role.Id);
                
                dbContext.Connection.Execute(sql, addRoleParams);
            }

            var restaurant = await _repository.CreateAsync(new Restaurant {
                Id = Guid.NewGuid().ToUuidString(),
                Status = RestaurantStatus.Running
            });

            _logger.LogError(param.CountryCodeId);
            var detail = await _repository.CreateAsync(new RestaurantDetail {
                Id = Guid.NewGuid().ToUuidString(),
                RestaurantId = restaurant.Id,
                CountryCodeId = param.CountryCodeId,
                RegistrationStatus = RestaurantRegistrationStatus.OwnerRegistered
            });

            var defaultMenu = await _repository.CreateAsync(new Menu {
                Id = Guid.NewGuid().ToUuidString(),
                Name = "Default Menu",
                BeginTime = 0,
                EndTime = 1440,
                RestaurantId = restaurant.Id,
                IsDefault = true
            });

            var defaultCategory = await _repository.CreateAsync(new DishCategory {
                Id = Guid.NewGuid().ToUuidString(),
                Name = "Default Category",
                Index = 1,
                MenuId = defaultMenu.Id,
                RestaurantId = restaurant.Id                
            });

            await _repository.CreateAsync(new UserExtension
            {
                Id = Guid.NewGuid().ToUuidString(),
                NickName = param.Name,
                UserId = userId,
                RestaurantId = restaurant.Id,
                CountryCodeId = param.CountryCodeId,
                LastLoginTime = null
            });

            using (var dbContext = new DapperDbContext(_configuration))
            {
                var sql = @"INSERT INTO [IdentitySchema].[UserClaims]
                            ([ClaimType] ,[ClaimValue] ,[UserId])
                            VALUES
                            (@resClaimType, @resClaimValue, @userId);";

                var addRoleParams = new DynamicParameters();

                addRoleParams.Add("resClaimType", JwtClaimType.RestaurantId);
                addRoleParams.Add("resClaimValue", restaurant.Id);
                addRoleParams.Add("userId", userId);

                dbContext.Connection.Execute(sql, addRoleParams);
            }

            await _repository.SaveAsync();

            return await RegisterToken(new FetchTokenParam
            {
                ClientId = "merchantMobile",
                ClientSecret = "secret",
                GrantType = "password",
                Phone = param.Phone,
                Password = param.Password,
                CountryCodeId = param.CountryCodeId
            });
        }


        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ApiResponse> Token([FromBody] FetchTokenParam param)
        {
            var parms = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", param.ClientId),
                new KeyValuePair<string, string>("client_secret", param.ClientSecret),
                new KeyValuePair<string, string>("grant_type", param.GrantType),
                new KeyValuePair<string, string>("username", param.Phone),
                new KeyValuePair<string, string>("password", param.Password)
            };

            var formContent = new FormUrlEncodedContent(parms);

            var response = await _httpClient.PostAsync("/connect/token", formContent);

            if (response.IsSuccessStatusCode)
            {
                var user = _readOnlyRepository.GetFirst<User>(u => u.PhoneNumber == param.Phone);
                
                var userExtension = await _readOnlyRepository.GetOneAsync<UserExtension>(u => u.UserId == user.Id);

                if (userExtension.CountryCodeId != param.CountryCodeId)
                {
                    return new ApiResponse
                    {
                        StatusCode = BusinessStatusCode.Failed,
                        Message = $"Wrong Country Code. "
                    };
                }

                if (userExtension.StaffStatus != null && userExtension.StaffStatus != StaffStatus.Normal)
                {
                    return new ApiResponse
                    {
                        StatusCode = BusinessStatusCode.Failed,
                        Message = $"Your account is not activated or freezed. "
                    };
                }

                var hasUserEverLogined = userExtension.LastLoginTime.HasValue;


                userExtension.LastLoginTime = DateTime.UtcNow;
                _repository.Update(userExtension);

                var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                    r.Id == userExtension.RestaurantId, null, "RestaurantDetail");

                var registrationStatus = restaurant.RestaurantDetail.RegistrationStatus;
                var isAutoReceiving = restaurant.RestaurantDetail?.IsReceivingAuto;

                var resultJson = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<FetchTokenResponse>(resultJson);
                data.RegistrationStatus = registrationStatus;
                data.HasUserEverLogined = hasUserEverLogined;
                data.IsAutoReceiving = isAutoReceiving;

                _repository.Save();
                return new ApiResponse {
                    StatusCode = BusinessStatusCode.Success,
                    Data = data
                };
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var tokenError = JsonConvert.DeserializeObject<TokenErrorResponse>(resultJson);
                if (tokenError != null)
                {
                    throw new ValidationException(!string.IsNullOrEmpty(tokenError.ErrorDescription)
                        ? tokenError.ErrorDescription
                        : tokenError.Error);
                }
            }

            throw
                new Exception($"response status code: {response.StatusCode.ToString()}; response content: { await response.Content.ReadAsStringAsync()}");
        }


        public async Task<ApiResponse> RegisterAsClient([FromBody] RegisterParam param)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .Select(e => e.First().ErrorMessage)
                .First();
                throw new BadRequestException(error);
            }

            //var verificationCode = await _readOnlyRepository.GetFirstAsync<VerificationCode>(c => c.Phone == param.Phone &&
            //                (DateTime.UtcNow - c.CreatedTime).TotalMinutes < 5,
            //                codes => codes.OrderByDescending(c => c.CreatedTime));

            //if (verificationCode == null)
            //{
            //    throw new Exception($"Invalid verification code. ");
            //}

            //if (verificationCode.Code != param.VerificationCode)
            //{
            //    throw new BadRequestException("Invalid verification code. ");
            //}

            var userId = Guid.NewGuid().ToUuidString();

            var result = await _userManager.CreateAsync(new User
            {
                Id = userId,
                UserName = param.Phone,
                PhoneNumber = param.Phone,
                Email = param.Email,
                PasswordHash = param.Password
            });

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(string.Join(",", errors));
            }

            var user = _readOnlyRepository.GetOne<User>(u => u.Id == userId);
            var role = _readOnlyRepository.GetOne<Role>(r => r.Name == AppConst.Client);

            using (var dbContext = new DapperDbContext(_configuration))
            {
                var sql = @"INSERT INTO [IdentitySchema].[UserRoles]
                            ([UserId] ,[RoleId])
                            VALUES
                            (@userId, @roleId);";

                var addRoleParams = new DynamicParameters();

                addRoleParams.Add("userId", userId);
                addRoleParams.Add("roleId", role.Id);

                dbContext.Connection.Execute(sql, addRoleParams);
            }

            await _repository.CreateAsync(new UserExtension
            {
                Id = Guid.NewGuid().ToUuidString(),
                NickName = param.Name,
                UserId = userId,
                CountryCodeId = param.CountryCodeId,
                LastLoginTime = null
            });

            await _repository.SaveAsync();

            return await RegisterToken(new FetchTokenParam
            {
                ClientId = "clientH5",
                ClientSecret = "secret",
                GrantType = "password",
                Phone = param.Phone,
                Password = param.Password,
                CountryCodeId = param.CountryCodeId
            });
        }

        public async Task<ApiResponse> TokenAsClient([FromBody] FetchTokenParam param)
        {
            var parms = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", param.ClientId),
                new KeyValuePair<string, string>("client_secret", param.ClientSecret),
                new KeyValuePair<string, string>("grant_type", param.GrantType),
                new KeyValuePair<string, string>("username", param.Phone),
                new KeyValuePair<string, string>("password", param.Password)
            };

            var formContent = new FormUrlEncodedContent(parms);

            var response = await _httpClient.PostAsync("/connect/token", formContent);

            if (response.IsSuccessStatusCode)
            {
                var user = _readOnlyRepository.GetFirst<User>(u => u.PhoneNumber == param.Phone);

                var userExtension = await _readOnlyRepository.GetOneAsync<UserExtension>(u => u.UserId == user.Id);

                if (userExtension.CountryCodeId != param.CountryCodeId)
                {
                    return new ApiResponse
                    {
                        StatusCode = BusinessStatusCode.Failed,
                        Message = $"Wrong Country Code. "
                    };
                }

                var hasUserEverLogined = userExtension.LastLoginTime.HasValue;


                userExtension.LastLoginTime = DateTime.UtcNow;
                _repository.Update(userExtension);

                var resultJson = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<FetchTokenResponse>(resultJson);
                _repository.Save();
                return new ApiResponse
                {
                    StatusCode = BusinessStatusCode.Success,
                    Data = data
                };
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var tokenError = JsonConvert.DeserializeObject<TokenErrorResponse>(resultJson);
                if (tokenError != null)
                {
                    throw new ValidationException(!string.IsNullOrEmpty(tokenError.ErrorDescription)
                        ? tokenError.ErrorDescription
                        : tokenError.Error);
                }
            }

            throw
                new Exception($"response status code: {response.StatusCode.ToString()}; response content: { await response.Content.ReadAsStringAsync()}");
        }


        private async Task<ApiResponse> RegisterToken(FetchTokenParam param)
        {
            var parms = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", param.ClientId),
                new KeyValuePair<string, string>("client_secret", param.ClientSecret),
                new KeyValuePair<string, string>("grant_type", param.GrantType),
                new KeyValuePair<string, string>("username", param.Phone),
                new KeyValuePair<string, string>("password", param.Password)
            };

            var formContent = new FormUrlEncodedContent(parms);

            var response = await _httpClient.PostAsync("/connect/token", formContent);

            if (response.IsSuccessStatusCode)
            {
                var user = _readOnlyRepository.GetFirst<User>(u => u.PhoneNumber == param.Phone);

                var userExtension = await _readOnlyRepository.GetOneAsync<UserExtension>(u => u.UserId == user.Id);
                _repository.Update(userExtension);

                var registrationStatus = (await _readOnlyRepository.GetFirstAsync<RestaurantDetail>(r => 
                    r.RestaurantId == userExtension.RestaurantId)).RegistrationStatus;

                var resultJson = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<FetchTokenResponse>(resultJson);
                data.RegistrationStatus = registrationStatus;
                data.HasUserEverLogined = false;

                _repository.Save();
                return new ApiResponse
                {
                    StatusCode = BusinessStatusCode.Success,
                    Data = data
                };
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var tokenError = JsonConvert.DeserializeObject<TokenErrorResponse>(resultJson);
                if (tokenError != null)
                {
                    throw new ValidationException(!string.IsNullOrEmpty(tokenError.ErrorDescription)
                        ? tokenError.ErrorDescription
                        : tokenError.Error);
                }
            }

            throw
                new Exception($"response status code: {response.StatusCode.ToString()}; response content: {response.Content}");
        }

        private bool IsEmailExist(string email)
        {
            var user = _readOnlyRepository.GetFirst<User>(u => u.Email == email);
            return user != null;
        }

        private bool IsPhoneExist(string phone)
        {
            var user = _readOnlyRepository.GetFirst<User>(u => u.PhoneNumber == phone);
            return user != null;
        }
    }
}
