using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using SFood.MerchantEndpoint.Application.Dtos.Results.Restaurant;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using SFood.MerchantEndpoint.Common.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly QiNiuOption _options;

        public RestaurantService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper,
            IOptionsSnapshot<QiNiuOption> options)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _options = options.Value;
        }

        /// <summary>
        /// 修改餐厅基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PostRestaurantBasicInfo(RestaurantBasicInfoParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => r.Id == param.RestaurantId, null, "RestaurantDetail");

            if (restaurant == null)
            {
                throw new Exception(param.RestaurantId);
            }

            restaurant.Name = param.Name;
            restaurant.CenterId = param.CenterId;
            restaurant.RestaurantDetail.RestaurantNo = param.RestaurantNo;
            restaurant.RestaurantDetail.RegistrationStatus = RestaurantRegistrationStatus.BasicInfoDone;

            _repository.Update(restaurant);

            //await _repository.CreateAsync(new RestaurantDetail
            //{
            //    Id = Guid.NewGuid().ToUuidString(),
            //    RestaurantId = restaurant.Id,
            //    RestaurantNo = param.RestaurantNo,
            //    RegistrationStatus = RestaurantRegistrationStatus.BasicInfoDone
            //});

            if (!param.CategoryIds.Any())
            {
                throw new BadRequestException("Categories is required, when uploading the basic information of restaurant");
            }

            var restaurant_RestaurantCategories = new List<Restaurant_RestaurantCategory>();

            param.CategoryIds.ForEach(categoryId =>
            {
                restaurant_RestaurantCategories.Add(
                    new Restaurant_RestaurantCategory
                    {
                        Id = Guid.NewGuid().ToUuidString(),
                        RestaurantId = param.RestaurantId,
                        RestaurantCategoryId = categoryId
                    });
            });
            await _repository.CreateRangeAsync(restaurant_RestaurantCategories);
        }

        /// <summary>
        /// 提交 修改餐厅审核信息
        /// </summary>
        /// <param name="param"></param>
        public async Task PostRestaurantQualificationInfo(QualificationInfoParam param)
        {
            if (param.ApplicationType == MerchantApplicationType.Company && param.CompanyInfo == null)
            {
                throw new BadRequestException("Company info needed, if you wanna register as company");
            }
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                r.Id == param.RestaurantId, null, "RestaurantDetail");

            var categoryIds = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(rrc =>
                rrc.RestaurantId == param.RestaurantId, null, "RestaurantCategory")).Select(rrc => 
                    rrc.RestaurantCategory.Id).ToList();

            restaurant.RestaurantDetail.ApplicationType = param.ApplicationType;
            restaurant.RestaurantDetail.RegistrationStatus = RestaurantRegistrationStatus.CertUploaded;
            _repository.Update(restaurant.RestaurantDetail);

            //creating qualification info for restaurant
            var qualifications = new List<Qualification>();

            if (param.ApplicationType == MerchantApplicationType.Company)
            {
                qualifications.Add(new Qualification {
                    Id = Guid.NewGuid().ToUuidString(),
                    Entry = MerchantQualificationEntry.CompanyName,
                    Value = param.CompanyInfo.Name
                });

                qualifications.Add(new Qualification
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    Entry = MerchantQualificationEntry.CompanyAddress,
                    Value = param.CompanyInfo.Address
                });

                qualifications.Add(new Qualification
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    Entry = MerchantQualificationEntry.CompanyBusinessScope,
                    Value = param.CompanyInfo.BusinessScope
                });
            }

            if (param.ApplicationType == MerchantApplicationType.Organization)
            {
                qualifications.Add(new Qualification
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    Entry = MerchantQualificationEntry.OrganizationCert,
                    Value = param.ImageUrls.FirstOrDefault(i => i.Type == MerchantQualificationEntry.OrganizationCert).Url
                });
            }

            foreach (var image in param.ImageUrls)
            {
                qualifications.Add(new Qualification
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    Entry = image.Type,
                    Value = image.Url
                });
            }

            
            _repository.Update(restaurant);
        }

        /// <summary>
        /// 修改餐厅的设置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PostRestaurantConfig(RestaurantConfigurationParam param)
        {
            var restaurantDetail = await _readOnlyRepository.GetFirstAsync<RestaurantDetail>(rd => rd.RestaurantId == param.RestaurantId, null, "Restaurant");

            if (restaurantDetail == null || restaurantDetail.Restaurant == null)
            {
                throw new Exception("None such data found in database.");
            }

            restaurantDetail.OpenedAt = param.OpenedAt;
            restaurantDetail.ClosedAt = param.ClosedAt;
            restaurantDetail.IsReceivingAuto = param.IsAutoReceiving;
            restaurantDetail.Restaurant.IsDeliverySupport = param.IsDeliverySupport;
            restaurantDetail.Restaurant.OrderResponseTime = param.OrderResponseTime;

            _repository.Update(restaurantDetail);
            _repository.Update(restaurantDetail.Restaurant);
        }

        /// <summary>
        /// 获取餐厅的设置
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<RestaurantConfigurationResult> GetRestaurantConfig(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => r.Id == restaurantId, null,
                "RestaurantDetail");

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            var result = new RestaurantConfigurationResult {
                OpenedAt = restaurant.RestaurantDetail.OpenedAt,
                ClosedAt = restaurant.RestaurantDetail.ClosedAt,
                OrderResponseTime = restaurant.OrderResponseTime,
                IsAutoReceiving = restaurant.RestaurantDetail.IsReceivingAuto,
                IsDeliverySupport = restaurant.IsDeliverySupport
            };
            return result;
        }

        /// <summary>
        /// 设置餐厅的资料信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PostRestaurantProfile(RestaurantProfileParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(rd => rd.Id == param.RestaurantId, null,
                "RestaurantDetail");

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            restaurant.Name = param.Name;
            restaurant.Logo = param.Logo;
            restaurant.Announcement = param.Announcement;
            restaurant.RestaurantDetail.RestaurantNo = param.RestaurantNo;
            restaurant.RestaurantDetail.CountryCodeId = param.CountryCodeId;
            restaurant.RestaurantDetail.Phone = param.Phone;
            restaurant.Logo = param.Logo;
            restaurant.RestaurantDetail.Introduction = param.Introduction;

            _repository.Update(restaurant);
            _repository.Update(restaurant.RestaurantDetail);

            var categories = _repository.GetAll<Restaurant_RestaurantCategory>()
                .Where(rrc => rrc.RestaurantId == param.RestaurantId).ToList();

            if (param.CategoryIds != null)
            {
                _repository.DeleteRange(categories);

                var newCategories = new List<Restaurant_RestaurantCategory>();

                param.CategoryIds.ForEach(categoryId => {
                    newCategories.Add(new Restaurant_RestaurantCategory
                    {
                        Id = Guid.NewGuid().ToUuidString(),
                        RestaurantId = param.RestaurantId,
                        RestaurantCategoryId = categoryId
                    });
                });
                await _repository.CreateRangeAsync(newCategories);
            }

            if (param.CategoriedImages != null)
            {
                var images = _readOnlyRepository.GetAll<Image>()
                    .Where(i => i.RestaurantId == param.RestaurantId &&
                        i.RestaurantImageCategory.HasValue).ToList();

                _repository.DeleteRange(images);

                var newImages = new List<Image>();

                param.CategoriedImages.ForEach(categoriedImages => {
                    categoriedImages.Images.ForEach(img => {
                        newImages.Add(new Image
                        {
                            Id = Guid.NewGuid().ToUuidString(),
                            RestaurantId = param.RestaurantId,
                            RestaurantImageCategory = categoriedImages.Category,
                            Url = img
                        });
                    });
                });
                await _repository.CreateRangeAsync(newImages);
            }
        }

        /// <summary>
        /// 修改店铺资料的基本信息
        /// </summary>
        /// <returns></returns>
        public async Task PostRestaurantRoughProfile(RestaurantRoughProfileParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(rd => rd.Id == param.RestaurantId, null, "RestaurantDetail");

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            restaurant.Name = param.Name;
            restaurant.Logo = param.Logo;
            restaurant.RestaurantDetail.RestaurantNo = param.RestaurantNo;
            restaurant.RestaurantDetail.CountryCodeId = param.CountryCodeId;
            restaurant.RestaurantDetail.Phone = param.Phone;
            restaurant.Logo = param.Logo;

            _repository.Update(restaurant);
            _repository.Update(restaurant.RestaurantDetail);

            var categories = _repository.GetAll<Restaurant_RestaurantCategory>()
                .Where(rrc => rrc.RestaurantId == param.RestaurantId).ToList();

            if (param.CategoryIds != null)
            {
                _repository.DeleteRange(categories);

                var newCategories = new List<Restaurant_RestaurantCategory>();

                param.CategoryIds.ForEach(categoryId => {
                    newCategories.Add(new Restaurant_RestaurantCategory
                    {
                        Id = Guid.NewGuid().ToUuidString(),
                        RestaurantId = param.RestaurantId,
                        RestaurantCategoryId = categoryId
                    });
                });
                await _repository.CreateRangeAsync(newCategories);
            }
        }

        /// <summary>
        /// 修改店铺图片
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PostRestaurantImages(RestaurantImagesParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(rd => rd.Id == param.RestaurantId);

            if (restaurant == null)
            {
                throw new Exception("None such data found in database.");
            }

            if (param.Categories != null)
            {
                var images = _readOnlyRepository.GetAll<Image>()
                    .Where(i => i.RestaurantId == param.RestaurantId &&
                        i.RestaurantImageCategory.HasValue).ToList();

                _repository.DeleteRange(images);

                var newImages = new List<Image>();

                param.Categories.ForEach(categoriedImages => {
                    categoriedImages.Images.ForEach(img => {
                        newImages.Add(new Image
                        {
                            Id = Guid.NewGuid().ToUuidString(),
                            RestaurantId = param.RestaurantId,
                            RestaurantImageCategory = categoriedImages.Category,
                            Url = img
                        });
                    });
                });
                await _repository.CreateRangeAsync(newImages);
            }
        }

        /// <summary>
        /// 获取餐厅资料信息
        /// </summary>
        /// <returns></returns>
        public async Task<RestaurantProfileResult> GetRestaurantProfile(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => r.Id == restaurantId, null,
                "RestaurantDetail,Center");            

            var categories = _readOnlyRepository.GetAll<Restaurant_RestaurantCategory>(null, "RestaurantCategory")
                .Where(c => c.RestaurantId == restaurantId).Select(c => new CategoryResult {
                    Id = c.RestaurantCategoryId,
                    Name = c.RestaurantCategory.Name
                }).ToList();

            var images = _readOnlyRepository.GetAll<Image>().
                Where(i => i.RestaurantId == restaurantId &&
                i.RestaurantImageCategory.HasValue).ToList();

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            var countryCode = await _readOnlyRepository.GetFirstAsync<CountryCode>(cc => 
                cc.Id == restaurant.RestaurantDetail.CountryCodeId);

            var result = new RestaurantProfileResult();
            result.Announcement = restaurant.Announcement;
            result.Introduction = restaurant.RestaurantDetail.Introduction;

            result.BasicInfo = new RestaurantBasicInfoResult {
                Name = restaurant.Name,
                Logo = restaurant.Logo,
                Phone = restaurant.RestaurantDetail.Phone,
                IsDeliverySupport = restaurant.IsDeliverySupport,
                HawkerCenterId = restaurant.CenterId,
                HawkerCenterName = restaurant.Center.Name,
                RestaurantNo = restaurant.RestaurantDetail.RestaurantNo,
                OpenedAt = restaurant.RestaurantDetail.OpenedAt,
                ClosedAt = restaurant.RestaurantDetail.ClosedAt,
                Categories = categories,
                Country = new CountryDto
                {
                    Id =  countryCode.Id,
                    Name = countryCode.Name,
                    Code = $"+{countryCode.Code.Trim()}",
                    FlagUrl = $"{_options.Domain}{countryCode.FlagUrl}" 
                }
            };

            result.Images = new List<CategoriedImagesResult> {
                new CategoriedImagesResult{
                    Category = RestaurantImageCategory.FrontDoor,
                    Images = images.Where(i => i.RestaurantImageCategory == RestaurantImageCategory.FrontDoor).Select(i => i.Url).ToList()
                },
                new CategoriedImagesResult{
                    Category = RestaurantImageCategory.Hall,
                    Images = images.Where(i => i.RestaurantImageCategory == RestaurantImageCategory.Hall).Select(i => i.Url).ToList()
                },new CategoriedImagesResult{
                    Category = RestaurantImageCategory.Kitchen,
                    Images = images.Where(i => i.RestaurantImageCategory == RestaurantImageCategory.Kitchen).Select(i => i.Url).ToList()
                },
                new CategoriedImagesResult{
                    Category = RestaurantImageCategory.Other,
                    Images = images.Where(i => i.RestaurantImageCategory == RestaurantImageCategory.Other).Select(i => i.Url).ToList()
                }
            };

            return result;
        }

        /// <summary>
        /// 修改餐厅公告
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="announcement"></param>
        /// <returns></returns>
        public async Task PostRestaurantAnnouncement(RestaurantAnnouncementParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(rd => rd.Id == param.RestaurantId);

            if (restaurant == null)
            {
                throw new Exception("None such data found in database.");
            }

            restaurant.Announcement = param.Announcement;

            _repository.Update(restaurant);
        }

        /// <summary>
        /// 修改餐厅介绍
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="announcement"></param>
        /// <returns></returns>
        public async Task PostRestaurantIntroduction(RestaurantIntroductionParam param)
        {
            var restaurantDetail = await _readOnlyRepository.GetFirstAsync<RestaurantDetail>(rd => rd.RestaurantId == param.RestaurantId);

            if (restaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            restaurantDetail.Introduction = param.Introduction;

            _repository.Update(restaurantDetail);
        }

        public async Task<RestaurantIndexResult> GetRestaurantIndexInfo(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetOneAsync<Restaurant>(r => r.Id == restaurantId,
                "RestaurantDetail");

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new Exception("None such data found in database.");
            }

            var result = new RestaurantIndexResult
            {
                Logo = restaurant.Logo,
                Name = restaurant.Name,
                AuditStatus = restaurant.RestaurantDetail.RegistrationStatus == RestaurantRegistrationStatus.Qualified ?
                    RestaurantAuditStatus.Audited : RestaurantAuditStatus.Auditing,
                Phone = restaurant.RestaurantDetail.Phone
            };
            return result;
        }

        /// <summary>
        /// 修改店铺的营业状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SwitchToOpen(SwitchOpenOrCloseParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
            r.Id == param.RestaurantId && r.Status == RestaurantStatus.Running);
            if (restaurant == null)
            {
                throw new Exception($"No such restaurant exist, restaurantId: {param.RestaurantId}");
            }
            restaurant.IsOpened = param.ToOpened;
            _repository.Update(restaurant);
        }

        public async Task<bool> GetOpenStatus(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r =>
            r.Id == restaurantId && r.Status == RestaurantStatus.Running);
            if (restaurant == null)
            {
                throw new Exception($"No such restaurant exist, restaurantId: {restaurantId}");
            }
            return restaurant.IsOpened;
        }

        public async Task<QualificationInfoResult> GetQualification(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                r.Id == restaurantId, null, "RestaurantDetail");

            var qualifications = (await _readOnlyRepository.GetAllAsync<Qualification>(q =>
                q.RestaurantId == restaurantId)).ToList();

            var restaurantCategories = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(rrc =>
                rrc.RestaurantId == restaurantId, null, "RestaurantCategory")).ToList();

            if (qualifications == null || !qualifications.Any())
            {
                return new QualificationInfoResult();
            }
            var result = new QualificationInfoResult {
                QualificationStatus = (restaurant.RestaurantDetail.RegistrationStatus.HasValue &&
                    restaurant.RestaurantDetail.RegistrationStatus.Value == RestaurantRegistrationStatus.Qualified) ? 1 : 2,
                NameEntry = new QualificationInfoResult.NameEntryDto
                {
                    Name = restaurant.Name
                },
                CategoryEntry = new QualificationInfoResult.CategoryEntryDto
                {
                    Categories = restaurantCategories.Select(rc => new QualificationInfoResult.CategoryDto {
                        Id = rc.RestaurantCategoryId,
                        Name = rc.RestaurantCategory.Name
                    }).ToList()
                },
                LocationEntry = new QualificationInfoResult.LocationEntryDto
                {
                    Name = restaurant.RestaurantDetail.RestaurantNo
                }
            };                        

            var qualificationList = qualifications.GroupBy(q => q.Entry, 
                q => q, 
                (entry, qList) => qList.OrderByDescending(x => x.CreatedTime).First());

            var certList = new List<QualificationInfoResult.CertDto>();
            var idFace = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.IdFace);
            certList.Add(new QualificationInfoResult.CertDto {
                Type = idFace.Entry,
                Value = idFace.Value,
                Status = idFace.IsQualified.HasValue ? (idFace.IsQualified.Value ? 1 : 3) : 2,
                Reason = idFace.Reason
            });

            var idBack = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.IdBack);
            certList.Add(new QualificationInfoResult.CertDto
            {
                Type = idBack.Entry,
                Value = idBack.Value,
                Status = idBack.IsQualified.HasValue ? (idBack.IsQualified.Value ? 1 : 3) : 2,
                Reason = idBack.Reason
            });

            var booth = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.Booth);
            certList.Add(new QualificationInfoResult.CertDto
            {
                Type = booth.Entry,
                Value = booth.Value,
                Status = booth.IsQualified.HasValue ? (booth.IsQualified.Value ? 1 : 3) : 2,
                Reason = booth.Reason
            });

            var foodSecurity = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.FoodSecurity);
            certList.Add(new QualificationInfoResult.CertDto
            {
                Type = foodSecurity.Entry,
                Value = foodSecurity.Value,
                Status = foodSecurity.IsQualified.HasValue ? (foodSecurity.IsQualified.Value ? 1 : 3) : 2,
                Reason = foodSecurity.Reason
            });

            var siteOfBusiness = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.SiteOfBusiness);
            certList.Add(new QualificationInfoResult.CertDto
            {
                Type = siteOfBusiness.Entry,
                Value = siteOfBusiness.Value,
                Status = siteOfBusiness.IsQualified.HasValue ? (foodSecurity.IsQualified.Value ? 1 : 3) : 2,
                Reason = siteOfBusiness.Reason
            });

            var organizationCert = qualificationList.FirstOrDefault(q => q.Entry == MerchantQualificationEntry.OrganizationCert);
            certList.Add(new QualificationInfoResult.CertDto
            {
                Type = organizationCert.Entry,
                Value = organizationCert.Value,
                Status = organizationCert.IsQualified.HasValue ? (organizationCert.IsQualified.Value ? 1 : 3) : 2,
                Reason = organizationCert.Reason
            });

            result.CertCollection = new QualificationInfoResult.CertCollectionDto
            {
                ApplicationType = (int) restaurant.RestaurantDetail.ApplicationType,
                Certs = certList
            };

            return result;
        }

        public async Task UpdateQualification(UpdateQualificationParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                r.Id == param.RestaurantId, null, "RestaurantDetail");

            var qualificationsToInsert = new List<Qualification>();
            if (!param.RestaurantName.IsNullOrWhiteSpace())
            {
                restaurant.Name = param.RestaurantName;
            }

            if (!param.RestaurantNo.IsNullOrEmpty())
            {
                restaurant.RestaurantDetail.RestaurantNo = param.RestaurantNo;
            }

            _repository.Update(restaurant);
            _repository.Update(restaurant.RestaurantDetail);

            if (param.Categories != null && param.Categories.Any())
            {
                var categories = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(rrc =>
                    rrc.RestaurantId == param.RestaurantId)).ToList();

                _repository.DeleteRange(categories);

                var categoryListToInsert = new List<Restaurant_RestaurantCategory>();
                foreach (var category in param.Categories)
                {
                    categoryListToInsert.Add(new Restaurant_RestaurantCategory {
                        Id = Guid.NewGuid().ToUuidString(),
                        RestaurantId = param.RestaurantId,
                        RestaurantCategoryId = category
                    });
                }

                await _repository.CreateRangeAsync(categoryListToInsert);
            }
            

            if (param.Certs != null && param.Certs.Any())
            {
                param.Certs.ForEach(cert => {
                    qualificationsToInsert.Add(new Qualification
                    {
                        Id = Guid.NewGuid().ToUuidString(),
                        RestaurantId = param.RestaurantId,
                        Entry = cert.Type,
                        Value = cert.Value
                    });
                });
            }

            await _repository.CreateRangeAsync(qualificationsToInsert);
        }
    }
} 