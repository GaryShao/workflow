using AutoMapper;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using SFood.MerchantEndpoint.Application.Dtos.Results.Dish;
using SFood.MerchantEndpoint.Application.Validator;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class DishService : IDishService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly ICustomizationValidator _customizationValidator;
        private readonly IDishValidator _dishValidator;
        private readonly IMapper _mapper;

        public DishService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            ICustomizationValidator customizationValidator,
            IDishValidator dishValidator,
            IMapper mapper)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _customizationValidator = customizationValidator;
            _dishValidator = dishValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// 创建菜品， 并和某菜单关联
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<CreateDishResult> PostDish(CreateDishParam param)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                r.Id == param.RestaurantId);

            if (restaurant == null)
            {
                throw new Exception("No such data exist in our db. ");
            }

            if (param.Categories == null || !param.Categories.Any())
            {
                throw new BadRequestException("Categories must be assigned. ");
            }

            var dishId = Guid.NewGuid().ToUuidString();
            var dish = new Dish
            {
                Id = dishId,
                Name = param.Name,
                UnitPrice = param.UnitPrice,
                Icon = param.Icon,
                RestaurantId = param.RestaurantId,
                CenterId = restaurant.CenterId
            };


            if (param.CustomizationCategoryIds != null && param.CustomizationCategoryIds.Any())
            {
                var customizationCategories = new List<Dish_CustomizationCategory>();

                param.CustomizationCategoryIds.ForEach(c => {
                    customizationCategories.Add(new Dish_CustomizationCategory {
                        Id = Guid.NewGuid().ToUuidString(),
                        DishId = dishId,
                        CustomizationCategoryId = c
                    });
                });

                dish.Dish_CustomizationCategories = customizationCategories;
            }

            if (param.Categories != null && param.Categories.Any())
            {
                var oldCategories = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc =>
                    ddc.DishId == dishId)).ToList();

                _repository.DeleteRange(oldCategories);

                var menuId = (await _readOnlyRepository.GetFirstAsync<DishCategory>(dc => dc.Id == param.Categories.First())).MenuId;

                await _repository.CreateAsync(new Dish_DishCategory
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    IsOnShelf = true,
                    Index = 0,
                    DishId = dishId,
                    MenuId = menuId,
                    DishCategoryId = param.Categories.FirstOrDefault(),
                    RestaurantId = param.RestaurantId
                });
            }

            var dishResult = await _repository.CreateAsync(dish);

            return new CreateDishResult
            {
                Id = dishResult.Id,
                Name = dishResult.Name,
                UnitPrice = dishResult.UnitPrice,
                Icon = dishResult.Icon
            };
        }

        public async Task PutDish(EditDishParam param)
        {
            var dish = await _readOnlyRepository.GetFirstAsync<Dish>(d => d.Id == param.Id);
            if (dish == null)
            {
                throw new BadRequestException($"There is no dish with id {param.Id}");
            }

            dish.Name = param.Name;
            dish.UnitPrice = param.UnitPrice;
            dish.Icon = param.Icon;

            _repository.Update(dish);

            var customizationCategories = param.CustomizationCategoryIds.Select(id => new Dish_CustomizationCategory {
                Id = Guid.NewGuid().ToUuidString(),
                DishId = param.Id,
                CustomizationCategoryId = id
            }).ToList();

            await _repository.CreateRangeAsync(customizationCategories);
        }

        public async Task DeleteDishes(DeleteDishParam param)
        {
            if (!param.DishIds.Any())
            {
                throw new BadRequestException("There is no dish ids at all in parameter");
            }

            await _dishValidator.ValidateDishIds(param.DishIds, param.RestaurantId);

            var dishGonnaDelete = (await _readOnlyRepository.GetAllAsync<Dish>(d => 
                param.DishIds.Contains(d.Id))).ToList();

            _repository.DeleteRange(dishGonnaDelete);

            var dish_Categories = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc => 
                ddc.RestaurantId == param.RestaurantId)).ToList();

            var categoriesGonaDelete = dish_Categories.Where(ca => param.DishIds.Contains(ca.DishId)).ToList();

            _repository.DeleteRange(categoriesGonaDelete);
        }

        public async Task<List<GetAllDishesResult>> GetAllDishes(string restaurantId)
        {
            var dishes = (await _readOnlyRepository.GetAllAsync<Dish>(d => d.RestaurantId == restaurantId,
                null, "Dish_DishCategories")).ToList();

            var menus = _readOnlyRepository.GetAll<Menu>().Where(m => m.RestaurantId == restaurantId).ToList();

            if (dishes == null || !dishes.Any())
            {
                return new List<GetAllDishesResult>();
            }

            var result = new List<GetAllDishesResult>();

            dishes.ForEach(d => {

                var menuIds = d.Dish_DishCategories.Select(ddc => ddc.MenuId).Distinct();

                var belongMenus = menus.Where(m => menuIds.Contains(m.Id));

                result.Add(new GetAllDishesResult
                {
                    Id = d.Id,
                    Name = d.Name,
                    UnitPrice = d.UnitPrice.ToMoneyString(),
                    Icon = d.Icon,
                    Menus = belongMenus.Select(bm => new MenuInfo
                    {
                        Name = bm.Name
                    }).ToList()
                });  
            });
            return result;
        }

        public async Task<List<DishBasicInfoResult>> GetAllDishesInCategory(string categoryId)
        {
            var category =  await _readOnlyRepository.
                    GetFirstAsync<DishCategory>(dc => dc.Id == categoryId);
            if (category == null)
            {
                throw new BadRequestException($"no such dish category found in db, categoryId : {categoryId}");
            }

            var dishes = _readOnlyRepository.GetAll<Dish_DishCategory>(null, "Dish").
                Where(ddc => ddc.DishCategoryId == categoryId).
                Select(d => new DishBasicInfoResult
                {
                    Id = d.DishId,
                    Name = d.Dish.Name,
                    Index = d.Index,
                    Icon = d.Dish.Icon,
                    UnitPrice = d.Dish.UnitPrice,
                    IsOnShelf = d.IsOnShelf
                }).ToList();

            if (dishes == null || !dishes.Any())
            {
                return new List<DishBasicInfoResult>();
            }
            else
            {
                return dishes;
            }
        }

        public async Task<DishResult> GetDish(string dishId)
        {
            var result = new DishResult();

            var dish = await _readOnlyRepository.GetFirstAsync<Dish>(d => d.Id == dishId);

            if (dish == null)
            {
                throw new BadRequestException($"There is no dish with id {dishId}");
            }

            result.Id = dish.Id;
            result.Name = dish.Name;
            result.UnitPrice = dish.UnitPrice.ToMoneyString();
            result.Icon = dish.Icon;

            var dishCategories = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(dc =>
                dc.DishId == dishId, null, "Category")).ToList();

            var menus = (await _readOnlyRepository.GetAllAsync<Menu>(m => dishCategories.Select(dc => dc.MenuId).Contains(m.Id))).ToList();

            result.Categories = new List<DishResult.CategoryDto>();

            if (dishCategories.Any())
            {
                foreach (var dishCategory in dishCategories)
                {
                    result.Categories.Add(new DishResult.CategoryDto
                    {
                        Id = dishCategory.Category.Id,
                        Name = dishCategory.Category.Name,
                        MenuId = dishCategory.MenuId,
                        MenuName = menus.First(m => m.Id == dishCategory.MenuId).Name
                    });
                }
            }

            var categoryIds = _readOnlyRepository.GetAll<Dish_CustomizationCategory>().
                Where(cc => cc.DishId == dishId).Select(ddc => ddc.CustomizationCategoryId).ToList();

            var customizations = _readOnlyRepository.GetAll<Customization>().
                Where(c => categoryIds.Contains(c.CategoryId)).ToList();

            if (categoryIds == null || !categoryIds.Any())
            {
                result.CustomizationCategories = new List<CustomizationCategoryDto>();
            }
            else
            {
                var categories = await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc =>
                    categoryIds.Contains(cc.Id));

                result.CustomizationCategories = categories.Select(category => new CustomizationCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSystem = category.IsSystem,
                    IsMultiple = category.IsMultiple,
                    IsSelected = category.IsSelected,
                    MaxSelected = category.MaxOptions,
                    Options = customizations.Where(custom => custom.CategoryId == category.Id).
                        Select(c => new CustomizationDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            UnitPrice = c.UnitPrice,
                            IsDefault = c.IsDefault
                        }).ToList()
                }).ToList();
            }

            return result;
        }

        public async Task<List<DishBasicInfoResult>> GetAllUnassignedDishes(GetUnassignedDishesParam param)
        {
            var dishes = (await _readOnlyRepository.GetAllAsync<Dish>(d => 
                    d.RestaurantId == param.RestaurantId)).ToList();

            if (!dishes.Any())
            {
                return new List<DishBasicInfoResult>();
            }

            var assignedDishIds = _readOnlyRepository.GetAll<Dish_DishCategory>().
                Where(ddc => ddc.MenuId == param.MenuId).
                Select(d => d.DishId).ToList();

            if (assignedDishIds.Any())
            {
                dishes.RemoveAll(d => assignedDishIds.Contains(d.Id));
            }            

            return dishes.Select(d => new DishBasicInfoResult {
                Id = d.Id,
                Name = d.Name,
                UnitPrice = d.UnitPrice,
                Icon = d.Icon                
            }).ToList();
        }

        public Task UpdateDishIndex(UpdateDishIndexParam param)
        {
            if (!param.DishIds.Any())
            {
                throw new BadRequestException($"no dish ids found in your request");
            }

            var dish_DishCategories = _readOnlyRepository.GetAll<Dish_DishCategory>().
                Where(ddc => ddc.DishCategoryId == param.CategoryId).ToList();

            var dishIdsInDb = dish_DishCategories.Select(ddc => ddc.DishId);

            var isBadRequest = param.DishIds.Any(id => !dishIdsInDb.Contains(id));

            if (isBadRequest)
            {
                throw new BadRequestException($"check your ids please, there are some categories that not belongs to the menu you provided. ");
            }

            byte index = 1;
            param.DishIds.ForEach(dishId => {
                var dish_DishCategory = dish_DishCategories.FirstOrDefault(ddc => ddc.DishId == dishId);
                dish_DishCategory.Index = index;
                index++;
            });

            _repository.UpdateRange(dish_DishCategories);
            return Task.CompletedTask;
        }
    }
}
