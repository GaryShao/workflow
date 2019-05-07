using AutoMapper;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos;
using SFood.MerchantEndpoint.Application.Dtos.Internal;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu;
using SFood.MerchantEndpoint.Application.Dtos.Results.Menu;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class MenuService : IMenuService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public MenuService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        public async Task Add(CreateMenuParam param)
        {
            var timeRanges = _readOnlyRepository.GetAll<Menu>().
                            Where(r => r.RestaurantId == param.RestaurantId).
                            Select(r => new TimeRange { BeginTime = r.BeginTime,  EndTime = r.EndTime });

            var thisTimeRange = new TimeRange
            {
                BeginTime = param.BeginTime,
                EndTime = param.EndTime
            };

            var isTimeRangeOverlapped = timeRanges != null &&
                timeRanges.Any(tr => IsTimeRangeOverlapped(tr, thisTimeRange));

            if (isTimeRangeOverlapped)
            {
                throw new BadRequestException("The time range of the menu which you're gonna create is overlapped others");
            }

            var newMenu = await _repository.CreateAsync(new Menu
            {
                Id = Guid.NewGuid().ToUuidString(),
                Name = param.Name,
                BeginTime = param.BeginTime,
                EndTime = param.EndTime,
                RestaurantId = param.RestaurantId
            });

            await _repository.CreateAsync(new DishCategory {
                Id = Guid.NewGuid().ToUuidString(),
                Name = "Default Category",
                Index = 1,
                MenuId = newMenu.Id,
                RestaurantId = param.RestaurantId
            });
        }

        public Task Edit(EditRecipeParam param)
        {
            var timeRanges = _readOnlyRepository.GetAll<Menu>().
                           Where(r => r.RestaurantId == param.RestaurantId && r.Id != param.Id).
                           Select(r => new TimeRange { BeginTime = r.BeginTime, EndTime = r.EndTime });

            var thisTimeRange = new TimeRange
            {
                BeginTime = param.BeginTime,
                EndTime = param.EndTime
            };

            var isTimeRangeOverlapped = timeRanges != null &&
                timeRanges.Any(tr => IsTimeRangeOverlapped(tr, thisTimeRange));

            if (isTimeRangeOverlapped)
            {
                throw new BadRequestException("The new time range of the menu which you're editing is overlapped others");
            }

            var recipe = _readOnlyRepository.GetOne<Menu>(r => r.Id == param.Id);
            recipe.Name = param.Name;
            recipe.BeginTime = param.BeginTime;
            recipe.EndTime = param.EndTime;

            _repository.Update(recipe);
            return Task.CompletedTask;
        }

        public async Task Delete(string menuId)
        {
            var categoryDishes = await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc =>
                ddc.MenuId == menuId);

            var categories = await _readOnlyRepository.GetAllAsync<DishCategory>(category =>
                category.MenuId == menuId);

            _repository.DeleteRange(categoryDishes.ToList());
            _repository.DeleteRange(categories.ToList());
            _repository.Delete<Menu>(menuId);
        }

        public async Task<MenuCategoryResult> AddDishCategory(CreateDishCategoryParam param)
        {
            if(param.MenuId.IsNullOrWhiteSpace())
            {
                //如果MenuId是null, 则向默认菜单添加分类

                var defaultMenu = await _readOnlyRepository.GetFirstAsync<Menu>(m =>
                        m.RestaurantId == param.RestaurantId && m.Name == "Default Menu");

                param.MenuId = defaultMenu.Id;
            }

            var indexes = _readOnlyRepository.GetAll<DishCategory>().
                            Where(dc => dc.MenuId == param.MenuId).
                            Select(dc => dc?.Index).ToList();

            var newIndex = (byte)((indexes?.Max() ?? 0) + 1);

            var category = await _repository.CreateAsync(new DishCategory {
                Id = Guid.NewGuid().ToUuidString(),
                Index = newIndex,
                Name = param.Name,
                MenuId = param.MenuId,
                RestaurantId = param.RestaurantId
            });

            return new MenuCategoryResult {
                Id = category.Id,
                Name = category.Name,
                Index = category.Index,
                CountOfDishes = 0
            };
        }

        public Task EditDishCategory(EditDishCategoryParam param)
        {
            var dishCategory = _readOnlyRepository.GetOne<DishCategory>(dc => dc.Id == param.Id);
            dishCategory.Name = param.Name;

            _repository.Update(dishCategory);
            return Task.CompletedTask;
        }

        public async Task DeleteDishCategory(string dishCategoryId)
        {
            var category = await _readOnlyRepository.GetFirstAsync<DishCategory>(c => c.Id == dishCategoryId);
            var isLast = (await _readOnlyRepository.GetCountAsync<DishCategory>(c => c.MenuId == category.MenuId)) == 1;
            if (isLast)
            {
                throw new BadRequestException($"The last category in each menu could not be deleted. ");
            }

            _repository.Delete<DishCategory>(dishCategoryId);

            var dish_DishCategories = _readOnlyRepository.GetAll<Dish_DishCategory>().
                    Where(ddc => ddc.DishCategoryId == dishCategoryId).ToList();
            _repository.DeleteRange(dish_DishCategories);
        }        

        public Task ChangeDishOnShelfStatus(DishStatusInRecipeParam param)
        {
            var dish_DishCategory = _readOnlyRepository.GetOne<Dish_DishCategory>(ddc => ddc.MenuId == param.RecipeId &&
                ddc.DishId == param.DishId);
            dish_DishCategory.IsOnShelf = param.SetAsOnShelf;

            _repository.Update(dish_DishCategory);
            return Task.CompletedTask;
        }

        public Task ChangeDishOnShelfStatusBatch(DishStatusBatchInRecipeParam param)
        {
            var allDish_DishCategories = _readOnlyRepository.GetAll<Dish_DishCategory>().Where(ddc => ddc.MenuId == param.MenuId);
            var dish_DishCategories =  param.DishIds.Join(allDish_DishCategories, id => id, ddc => ddc.DishId, (id, ddc) => ddc).ToList();

            foreach (var dish_DishCategory in dish_DishCategories)
            {
                dish_DishCategory.IsOnShelf = param.SetAsOnShelf;
            }

            _repository.UpdateRange(dish_DishCategories);
            return Task.CompletedTask;
        }

        public async Task<List<MenuRoughResult>> GetAll(string restaurantId)
        {
            var menus = await _readOnlyRepository.GetAllAsync<Menu>(m => m.RestaurantId == restaurantId);

            var categories = await _readOnlyRepository.GetAllAsync<DishCategory>(rrc => rrc.RestaurantId == restaurantId);

            var dishes = await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc => ddc.RestaurantId == restaurantId);

            if (menus == null || !menus.Any())
            {
                return new List<MenuRoughResult>();
            }

            return menus.Select(m => new MenuRoughResult
            {
                Id = m.Id,
                Name = m.Name,
                BeginTime = m.BeginTime,
                EndTime = m.EndTime,
                CountOfCategories = categories.Where(c => c.MenuId == m.Id)?.Count() ?? default(int),
                CountOfDishes = dishes.Where(d => d.MenuId == m.Id)?.Count() ?? default(int),
                IsCurrent = IsCurrent(m.BeginTime, m.EndTime)
            }).ToList();
        }

        public async Task<MenuDetailResult> GetDetail(string menuId)
        {
            var menu = await _readOnlyRepository.GetFirstAsync<Menu>(m => m.Id == menuId);
            if (menu == null)
            {
                throw new BadRequestException($"no such menu in db. menu id : {menuId}");
            }
            var result = new MenuDetailResult();
            result.Id = menu.Id;
            result.Name = menu.Name;
            result.BeginTime = menu.BeginTime;
            result.EndTime = menu.EndTime;
            result.IsCurrent = IsCurrent(menu.BeginTime, menu.EndTime);

            var dishCategories = _readOnlyRepository.GetAll<DishCategory>().
                Where(dc => dc.MenuId == menuId).OrderBy(dc => dc.Index).ToList();

            if (dishCategories == null || !dishCategories.Any())
            {
                result.CountOfCategories = 0;
                result.CountOfDishes = 0;

                return result;
            }

            var dish_DishCategories = _readOnlyRepository.GetAll<Dish_DishCategory>(null, "Dish").
                Where(ddc => ddc.MenuId == menuId).ToList();

            var categories = new List<MenuDetail_CategoryResult>();
            dishCategories.ForEach(c => {
                var category = new MenuDetail_CategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Index = c.Index,
                };
                var dishes_InCurrentCategory = dish_DishCategories.Where(ddc => ddc.DishCategoryId == c.Id).
                    OrderBy(ddc => ddc.Index);
                if (dishes_InCurrentCategory != null && dishes_InCurrentCategory.Any())
                {
                    category.Dishes.AddRange(dishes_InCurrentCategory.Where(ddc => ddc.Dish != null).Select(dicc => new MenuDetail_DishResult {
                        Id = dicc.DishId,
                        Name = dicc.Dish.Name,
                        Icon = dicc.Dish.Icon,
                        UnitPrice = dicc.Dish.UnitPrice.ToMoneyString(),
                        Index = dicc.Index,
                        IsOnShelf = dicc.IsOnShelf
                    }));
                }
                categories.Add(category);
            });
            result.Categories = categories;
            result.CountOfCategories = categories.Count();
            result.CountOfDishes = categories.Sum(c => c.Dishes.Count());
            return result;
        }

        public async Task<AllMenuDetailResult> GetAllDetail(string restaurantId)
        {
            var result = new AllMenuDetailResult {
                IsThereAnyDishes = true
            };

            var dishes = await _readOnlyRepository.GetAllAsync<Dish>(d => d.RestaurantId == restaurantId);

            if (dishes == null || !dishes.Any())
            {
                result.IsThereAnyDishes = false;
            }

            var menus = await _readOnlyRepository.GetAllAsync<Menu>(m => m.RestaurantId == restaurantId);

            if (menus == null || !menus.Any())
            {
                return result;
            }

            menus.ForEach(m => {
                result.Menus.Add(GetDetail(m.Id).Result);
            });
            return result;
        }

        public async Task<List<RoughMenuInfoResult>> GetAllRoughMenuInfo(string restaurantId)
        {
            var menus = await _readOnlyRepository.GetAllAsync<Menu>(m => 
                m.RestaurantId == restaurantId);
            if (menus == null || !menus.Any())
            {
                return new List<RoughMenuInfoResult>();
            }
            var result = new List<RoughMenuInfoResult>();

            var categories = await _readOnlyRepository.GetAllAsync<DishCategory>(dc => 
                dc.RestaurantId == restaurantId);

            menus.ForEach(menu => {
                result.Add(new RoughMenuInfoResult {
                    Id = menu.Id,
                    Name = menu.Name,
                    Categories = categories.Where(c => c.MenuId == menu.Id).
                        Select(c => new CategoryInfo_RoughMenu {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList()
                });
            });

            return result;
        }

        /// <summary>
        /// 复制菜单
        /// </summary>
        /// <returns></returns>
        public async Task Replicate(ReplicaParam param)
        {
            var timeRanges = _readOnlyRepository.GetAll<Menu>().
                            Where(r => r.RestaurantId == param.RestaurantId).
                            Select(r => new TimeRange { BeginTime = r.BeginTime, EndTime = r.EndTime });

            var thisTimeRange = new TimeRange
            {
                BeginTime = param.BeginTime,
                EndTime = param.EndTime
            };

            var isTimeRangeOverlapped = timeRanges != null &&
                timeRanges.Any(tr => IsTimeRangeOverlapped(tr, thisTimeRange));

            if (isTimeRangeOverlapped)
            {
                throw new BadRequestException("The time range of the menu which you're gonna create is overlapped others");
            }

            var newMenu = await _repository.CreateAsync(new Menu
            {
                Id = Guid.NewGuid().ToUuidString(),
                Name = param.Name,
                BeginTime = param.BeginTime,
                EndTime = param.EndTime,
                RestaurantId = param.RestaurantId
            });

            var categories = (await _readOnlyRepository.GetAllAsync<DishCategory>(dc =>
                dc.MenuId == param.MenuId)).ToList();

            var categoryIdMapper = new Dictionary<string, string>();
            var newCategories = new List<DishCategory>();
            var newDishcategories = new List<Dish_DishCategory>();

            categories.ForEach(category => {
                var newCategoryId = Guid.NewGuid().ToUuidString();

                newCategories.Add(new DishCategory {
                    Id = newCategoryId,
                    Name = category.Name,
                    Index = category.Index,
                    RestaurantId = category.RestaurantId,
                    MenuId = newMenu.Id,
                    CreatedTime = DateTime.UtcNow
                });

                categoryIdMapper[category.Id] = newCategoryId;
            });

            await _repository.CreateRangeAsync(newCategories);

            var categoryDishes = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(
                ddc => ddc.MenuId == param.MenuId)).ToList();

            categoryDishes.ForEach(categoryDish => {
                newDishcategories.Add(new Dish_DishCategory {
                    Id = Guid.NewGuid().ToUuidString(),
                    Index = categoryDish.Index,
                    IsOnShelf = categoryDish.IsOnShelf,
                    DishId = categoryDish.DishId,
                    DishCategoryId = categoryIdMapper[categoryDish.DishCategoryId],
                    MenuId = newMenu.Id,
                    RestaurantId = categoryDish.RestaurantId
                });
            });

            await _repository.CreateRangeAsync(newDishcategories);
        }

        /// <summary>
        /// 判断2个时间段是否有重叠
        /// </summary>
        /// <param name="a">时间段a</param>
        /// <param name="b">时间段b</param>
        /// <returns></returns>
        private bool IsTimeRangeOverlapped(TimeRange a, TimeRange b)
        {
            var situationA = b.BeginTime >= a.BeginTime && b.BeginTime < a.EndTime;
            var situationB = b.EndTime <= a.EndTime && b.EndTime >= a.BeginTime;

            return situationA || situationB;
        }

        private bool IsCurrent(short begin, short end)
        {
            var isUnix = Environment.OSVersion.Platform == PlatformID.Unix;
            var zoneId = isUnix ? "Asia/Singapore" : "Singapore Standard Time";
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

            var beginOfToday = DateTime.Now.Date;
            var minutes = (now - beginOfToday).TotalMinutes;

            return minutes >= begin && minutes <= end;
        }
    }
}
