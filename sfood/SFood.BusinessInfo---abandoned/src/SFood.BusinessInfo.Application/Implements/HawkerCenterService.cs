using AutoMapper;
using SFood.BusinessInfo.Application.Dtos.Parameters;
using SFood.BusinessInfo.Application.Dtos.Responses;
using SFood.BusinessInfo.Common.Exceptions;
using SFood.BusinessInfo.Common.Extensions;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFood.BusinessInfo.Application.Implements
{
    public class HawkerCenterService : IHawkerCenterService
    {
        private readonly IReadOnlyRepository _readonlyRepository;
        private readonly IMapper _mapper;

        public HawkerCenterService(IReadOnlyRepository readonlyRepository
            ,IMapper mapper)
        {
            _readonlyRepository = readonlyRepository;
            _mapper = mapper;
        }

        public IEnumerable<Restaurant> RetrieveRestaurants(RetrieveRestaurantsParam  param)
        {
            var query = _readonlyRepository.GetAll<Restaurant>();
            if (!param.SortBy.IsNullOrWhiteSpace())
            {
                query = query.OrderBy(r => r.SortWeight);
            }
            if (!param.CategoryId.IsNullOrWhiteSpace())
            {
                query = query.Where(q => q.CategoryId == param.CategoryId);
            }
            if (!param.SearchWord.IsNullOrWhiteSpace())
            {
                query = query.Where(q => q.Name.Contains(param.SearchWord));
            }
            return query;
        }

        public IEnumerable<RestaurantCategoryDto> RetrieveRestaurantCategories(string centerId)
        {
            var categories = _readonlyRepository.GetAll<RestaurantCategory>().
                Where(rc => rc.CenterId == centerId).
                Select(c => _mapper.Map<RestaurantCategoryDto>(c));
            return categories;
        }

        public CenterRoughInfoDto GetCenterRoughtInfo(string centerId, string seatId)
        {
            if (centerId.IsNullOrWhiteSpace())
            {
                throw new BadRequestException("CenterId is required");
            }

            var centers = _readonlyRepository.GetAll<HawkerCenter>().Where(c => c.Id == centerId);

            if (centers == null)
            {
                throw new DataNotFoundException($"Hawker center with id {centerId} not found.");
            }

            var seats = _readonlyRepository.GetAll<Seat>().Where(s => s.Id == seatId);

            var restaurantCategories = _readonlyRepository.GetAll<RestaurantCategory>().Where(rc => rc.CenterId == centerId);
            var adBanners = _readonlyRepository.GetAll<Image>().Where(i => i.CenterId == centerId);


            var roughInfo = (from center in centers
                      join category in restaurantCategories
                      on center.Id equals category.CenterId into center_categories
                      from center_category in center_categories.DefaultIfEmpty()

                      join banner in adBanners
                      on center.Id equals banner.CenterId into center_Banners
                      from center_banner in center_Banners.DefaultIfEmpty()

                      join seat in seats
                      on center.Id equals seat.CenterId into center_seats
                      from center_seat in center_seats.DefaultIfEmpty()

                      group new { center, center_category, center_banner, center_seat } by center
                      into query
                      select new CenterRoughInfoDto {
                            CenterId = query.Key.Id,
                            Name = query.Key.Name,
                            SeatName = query.FirstOrDefault(q => q.center_seat != null).center_seat.Name,
                            Images = query.Where(q => !q.center_banner.IsDeleted).
                                Select(q => q.center_banner.Url).Distinct(),
                            Categories = query.Where(q => !q.center_category.IsDeleted).
                                Select(q => new RestaurantCategoryDto {
                                    Id = q.center_category.Id,
                                    Name = q.center_category.Name,
                                    Icon = q.center_category.Icon,
                                    SelectedIcon = q.center_category.SelectedIcon
                                })
                       }).FirstOrDefault();

            return roughInfo;
        }
    }
}
