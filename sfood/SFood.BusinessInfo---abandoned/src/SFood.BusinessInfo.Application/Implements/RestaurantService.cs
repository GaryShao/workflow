using AutoMapper;
using SFood.BusinessInfo.Application.Dtos.Responses;
using SFood.BusinessInfo.Common.Exceptions;
using SFood.BusinessInfo.Common.Extensions;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using System.Collections.Generic;
using System.Linq;

namespace SFood.BusinessInfo.Application.Implements
{
    public class RestaurantService: IRestaurantService
    {
        private readonly IReadOnlyRepository _readonlyRepository;
        private readonly IMapper _mapper;

        public RestaurantService(IReadOnlyRepository readonlyRepository
            ,IMapper mapper)
        {
            _readonlyRepository = readonlyRepository;
            _mapper = mapper;
        }

        public IEnumerable<DishCategoryRoughDto> GetDishCategories(string recipeId)
        {
            if (recipeId.IsNullOrWhiteSpace())
            {
                throw new BadRequestException($"recipeId is Required");
            }
            var categories = _readonlyRepository.GetAll<DishCategory>().
                Where(rdc => rdc.RecipeId == recipeId).
                Select(c => _mapper.Map<DishCategoryRoughDto>(c));

            return categories;
        }

        public IEnumerable<DishCategoryDetailDto> GetAllDishes(string recipeId)
        {
            var categories = GetDishCategories(recipeId);
            var dish_DishCateries = _readonlyRepository.GetAll<Dish_DishCategory>(null, "Dish");

            var query = from category in categories
                        join dish_DishCategory in dish_DishCateries
                        on category.Id equals dish_DishCategory.DishCategoryId
                        group new { category, dish_DishCategory } by category
                        into temp
                        select new DishCategoryDetailDto
                        {
                            CategoryId = temp.Key.Id,
                            Name = temp.Key.Name,
                            Dishes = temp.Select(t => _mapper.Map<DishDto>(t.dish_DishCategory.Dish))
                        };

            return query;
        }

        public RestaurantProfileDto GetProfile(string restaurantId)
        {
            if (restaurantId.IsNullOrWhiteSpace())
            {
                throw new BadRequestException("RestaurantId is required!");
            }

            var restaurants = _readonlyRepository.GetAll<Restaurant>(null, "RestaurantDetail, Center").
                    Where(r => r.Id == restaurantId);

            var images = _readonlyRepository.GetAll<Image>().
                    Where(i => i.RestaurantId == restaurantId);

            var restaurantProfile = restaurants.GroupJoin(images, 
                res => res.Id, 
                img => img.RestaurantId, 
                (res, imgs) => new RestaurantProfileDto {
                    Id = res.Id,
                    Name = res.Name,
                    Logo = res.Logo,
                    IsOpened = res.IsOpened,
                    IsDeliverySupport = res.IsDeliverySupport,
                    OpenedAt = res.RestaurantDetail.OpenedAt.ToStringTime(),
                    ClosedAt = res.RestaurantDetail.ClosedAt.ToStringTime(),
                    RestaurantNo = res.RestaurantDetail.RestaurantNo,
                    Phone = res.RestaurantDetail.Phone,
                    Introduction = res.RestaurantDetail.Introduction,
                    CenterName = res.Center.Name,
                    IsShowByTime = res.RestaurantDetail.IsShowByTime,
                    IsReceivingAuto = res.RestaurantDetail.IsReceivingAuto,
                    Images = CategoriseImages(imgs)
            }).FirstOrDefault();

            return restaurantProfile;
        }

        private IEnumerable<CategorisedRestaurantImagesDto> CategoriseImages(IEnumerable<Image> images)
        {
            return images.
                    GroupBy(i => i.RestaurantLogoCategory).
                    Select(groupedImages => new CategorisedRestaurantImagesDto {
                        Category = groupedImages.Key.Value,
                        Images = groupedImages.Select(i => i.Url)
                    });
        }
    }
}