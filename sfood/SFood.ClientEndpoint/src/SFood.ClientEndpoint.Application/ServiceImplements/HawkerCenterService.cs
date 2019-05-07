using Microsoft.Extensions.Options;
using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.Dtos.Result;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Common.Exceptions;
using SFood.ClientEndpoint.Common.Extensions;
using SFood.ClientEndpoint.Common.Options;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFood.ClientEndpoint.Application.ServiceImplements
{
    public class HawkerCenterService : IHawkerCenterService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly QiNiuOption _options;
        private readonly ILogger<HawkerCenterService> _logger;

        public HawkerCenterService(IReadOnlyRepository readOnlyRepository,
            IOptionsSnapshot<QiNiuOption> options, ILogger<HawkerCenterService> logger)
        {
            _readOnlyRepository = readOnlyRepository;
            _logger = logger;
            _options = options.Value; 
        }

        public async Task<IEnumerable<RestaurantResult>> GetRestaurants(GetRestaurantsParam param)
        {
            Func<IQueryable<Restaurant>, IOrderedQueryable<Restaurant>> orderedExpress = (res) => {
                switch (param.SortType)
                {
                    case GetRestaurantsParam.SortingType.Turnover:
                        return res.OrderBy(r => r.SalesVolumeAnnual);
                    case GetRestaurantsParam.SortingType.Default:
                        return res.OrderBy(r => r.SortWeight);
                    default:
                        throw new BadRequestException("No such sorting type. ");
                }
            };
            //todo: certupload to qualifed. 
            Func<Restaurant, bool> filterExpress = (res) => {                
                var constraint1 = res.CenterId == param.CenterId &&
                    res.RestaurantDetail != null &&
                    res.RestaurantDetail.RegistrationStatus == RestaurantRegistrationStatus.CertUploaded;
                var constraint2 = true;
                var constraint3 = true;

                if (!param.SearchWord.IsNullOrEmpty())
                {
                    constraint2 = res.Name.Contains(param.SearchWord);
                }
                if (param.CategoryIds != null && param.CategoryIds.Any())
                {
                    constraint3 = res.Restaurant_RestaurantCategories.Select(rrc =>
                        rrc.RestaurantCategoryId).Join(param.CategoryIds, o => o, i => i, (i, o) => i).Count() > 0;
                }
                return constraint1 && constraint2 && constraint3;
            };

            var allRestaurants = _readOnlyRepository.GetAll<Restaurant>(res =>
                orderedExpress(res), "RestaurantDetail,Restaurant_RestaurantCategories").Where(r => r.CenterId == param.CenterId).ToList();

            var restaurants = allRestaurants.Where(r => filterExpress(r)).Skip(param.PageIndex * param.PageSize).Take(param.PageSize);

            var categories = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(null, null, "RestaurantCategory")).ToList();

            if (restaurants == null)
            {
                throw new Exception("None data found in database.");
            }

            var result = restaurants.Select(r => new RestaurantResult
            {
                Id = r.Id,
                Name = r.Name,
                Logo = r.Logo,
                Announcement = r.Announcement,
                IsOpened = r.IsOpened,
                IsDeliverySupport = r.IsDeliverySupport,
                Categories = categories.Where(c => c.RestaurantId == r.Id).Select(c => c.RestaurantCategoryId).ToList()
            });

            return result;
        }

        public async Task<CenterRoughDto> GetCenterRough(GetCenterParam param)
        {  
            var center = await _readOnlyRepository.GetFirstAsync<HawkerCenter>(hc => 
                hc.Id == param.CenterId);

            Seat seat = null;

            if (center == null)
            {
                throw new BadRequestException($"No such data found. ");
            }

            if (!param.SeatId.IsNullOrEmpty())
            {
                seat = await _readOnlyRepository.GetFirstAsync<Seat>(s => 
                    s.Id == param.SeatId);

                if (seat == null)
                {
                    throw new BadRequestException($"No such data found. ");
                }
            }
            

            var categories = await _readOnlyRepository.GetAllAsync<RestaurantCategory>();
            var adBanners = await _readOnlyRepository.GetAllAsync<HawkerCenterBanner>(b => 
                b.CenterId == param.CenterId);

            var result = new CenterRoughDto
            {
                Id = center.Id,
                Name = center.Name,
                SeatName = seat?.Name,
                Banners = adBanners.Select(b => b.TargetUrl),
                Categories = categories.Select(c => new RestaurantCategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Icon = c.Icon,
                    SelectedIcon = c.SelectedIcon
                })
            };            
            return result;
        }
    }
}
