using Microsoft.Extensions.Options;
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

namespace SFood.ClientEndpoint.Application.ServiceImplements
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly QiNiuOption _options;

        public RestaurantService(IReadOnlyRepository readOnlyRepository,
            IOptionsSnapshot<QiNiuOption> options)
        {
            _readOnlyRepository = readOnlyRepository;
            _options = options.Value;
        }

        /// <summary>
        /// 获取当前菜单的所有分类及菜品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<MenuContentResult> GetCurrentMenuContent(string restaurantId)
        {
            var menu = await GetCurrentMenu(restaurantId);
            if (menu == null)
            {
                menu = await GetNextMenu(restaurantId);
            }

            var categories = (await _readOnlyRepository.GetAllAsync<DishCategory>(category =>
                category.MenuId == menu.Id, ca => ca.OrderBy(c => c.Index))).ToList();
            if (categories == null)
            {
                throw new Exception($"No such data exist. ");
            }

            var dish_DishCateries = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc => 
                ddc.MenuId == menu.Id, null, "Dish")).ToList();

            var dish_CustomizationCategories = (await _readOnlyRepository.GetAllAsync<Dish_CustomizationCategory>(dcc =>
                dish_DishCateries.Select(ddc => ddc.DishId).Contains(dcc.DishId))).ToList();

            var result = new MenuContentResult();

            categories.ForEach(category => {
              result.Categories.Add(new MenuContentResult.CategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Index = category.Index,
                    Dishes = dish_DishCateries.Where(ddc => ddc.DishCategoryId == category.Id && ddc.IsOnShelf && ddc.Dish != null)
                        .OrderBy(c => c.Index).Select(c => new MenuContentResult.DishDto {
                            Id = c.Dish.Id,
                            Name = c.Dish.Name,
                            Index = c.Index,
                            Logo = c.Dish.Icon,
                            UnitPrice = c.Dish.UnitPrice,
                            SaleVolume = c.Dish.SalesVolumeAnnual,
                            HasCustomization = dish_CustomizationCategories.Select(d => d.DishId).Contains(c.DishId)
                        })
                });
            });
            return result;
        }

        /// <summary>
        /// 获取餐厅详情
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<RestaurantProfileResult> GetRestaurantProfile(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r =>
                r.Id == restaurantId, null, "RestaurantDetail,Center");

            if (restaurant == null || restaurant.RestaurantDetail == null)
            {
                throw new BadRequestException("No such data exist. ");
            }

            var profile = new RestaurantProfileResult
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Logo = restaurant.Logo,
                IsOpened = restaurant.IsOpened,
                IsDeliverySupport = restaurant.IsDeliverySupport,
                OpenedAt = restaurant.RestaurantDetail.OpenedAt ?? default(short),
                ClosedAt = restaurant.RestaurantDetail.ClosedAt ?? default(short),
                RestaurantNo = restaurant.RestaurantDetail.RestaurantNo,
                Phone = restaurant.RestaurantDetail.Phone,
                Introduction = restaurant.RestaurantDetail.Introduction,
                CenterName = restaurant.Center.Name,
                IsReceivingAuto = restaurant.RestaurantDetail.IsReceivingAuto
            };

            var images = (await _readOnlyRepository.GetAllAsync<Image>(i => 
                i.RestaurantId == restaurantId)).ToList();

            images.GroupBy(i => i.RestaurantImageCategory).ForEach(g => {
                profile.Images.Add(new RestaurantProfileResult.ImagesDto {
                    Category = GetImageCategoryName(g.Key.Value),
                    Images = g.Select(x => x.Url)
                });
            });

            return profile;
        }

        /// <summary>
        /// 获取某个菜品的规格详情
        /// </summary>
        /// <param name="dishId">菜品id</param>
        /// <returns></returns>
        public async Task<GetCustomizationsResult> GetCustomizations(string dishId)
        {
            var dish = await _readOnlyRepository.GetFirstAsync<Dish>(d => 
                d.Id == dishId);

            if (dish == null)
            {
                throw new BadRequestException("No such data exist. ");
            }

            var categoryIds = (await _readOnlyRepository.GetAllAsync<Dish_CustomizationCategory>(dcc =>
                dcc.DishId == dishId)).Select(dcc => dcc.CustomizationCategoryId).ToList();

            var categories = await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc =>
                categoryIds.Contains(cc.Id), null, "Customizations");

            var result = new GetCustomizationsResult();
            result.Id = dishId;
            result.Name = dish.Name;
            result.UnitPrice = dish.UnitPrice;

            foreach (var category in categories)
            {
                var categoryResult = new CategoryResult
                {
                    Id = category.Id,
                    Name = category.Name,
                    MaxSelected = category.MaxOptions,
                    IsMultiple = category.IsMultiple,
                    IsSelected = category.IsSelected
                };
                categoryResult.Options = category.Customizations.Select(c => new OptionResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Index = c.Index,
                    UnitPrice = c.UnitPrice.ToMoneyString(),
                    IsDefault = c.IsDefault
                }).ToList();

                result.Categories.Add(categoryResult);
            }
            return result;
        }


        public async Task<List<MenuContentResult.DishDto>> GetDishes(string restaurantId, string searchWord=null)
        {
            var dishes = (await _readOnlyRepository.GetAllAsync<Dish>(d =>
                d.RestaurantId == restaurantId));

            if (!string.IsNullOrWhiteSpace(searchWord))
                dishes = dishes.Where(d => d.Name.Contains(searchWord.Trim()));

            var dish_CustomizationCategories = (await _readOnlyRepository.GetAllAsync<Dish_CustomizationCategory>(dcc =>
                dishes.Select(ddc => ddc.Id).Contains(dcc.DishId))).ToList();

            return dishes.Select(d => new MenuContentResult.DishDto
            {
                Id = d.Id,
                Name = d.Name,
                Logo = d.Icon,
                UnitPrice = d.UnitPrice,
                SaleVolume = d.SalesVolumeAnnual,
                HasCustomization = dish_CustomizationCategories.Select(dcc => dcc.DishId).Contains(d.Id)
            }).ToList();
        }

        private string GetImageCategoryName(RestaurantImageCategory category)
        {
            switch (category)
            {
                case RestaurantImageCategory.FrontDoor:
                    return "前台";
                case RestaurantImageCategory.Hall:
                    return "大堂";
                case RestaurantImageCategory.Kitchen:
                    return "厨房";
                case RestaurantImageCategory.Other:
                    return "其他";
                default:
                    throw new Exception("No such data exist");
            }
        }

        private async Task<Menu> GetCurrentMenu(string restaurantId)
        {
            var isUnix = Environment.OSVersion.Platform == PlatformID.Unix;
            var zoneId = isUnix ? "Asia/Singapore" : "Singapore Standard Time";
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var hour = now.Hour;
            var mins = now.Minute;
            var nowMins = hour * 60 + mins;

            var menu = await _readOnlyRepository.GetFirstAsync<Menu>(m =>
                m.RestaurantId == restaurantId && nowMins >= m.BeginTime && nowMins < m.EndTime);
            return menu;
        }

        private async Task<Menu> GetNextMenu(string restaurantId)
        {
            var menu = await _readOnlyRepository.GetFirstAsync<Menu>(m =>
                m.RestaurantId == restaurantId && m.BeginTime == 0);
            return menu;
        }
    }
}
