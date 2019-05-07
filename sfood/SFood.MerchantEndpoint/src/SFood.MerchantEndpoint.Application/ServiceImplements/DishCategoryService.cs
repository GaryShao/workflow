using AutoMapper;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory;
using SFood.MerchantEndpoint.Application.Dtos.Results;
using SFood.MerchantEndpoint.Application.Dtos.Results.DishCategory;
using SFood.MerchantEndpoint.Application.Validator;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class DishCategoryService : IDishCategoryService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IDishValidator _dishValidator;
        private readonly IMapper _mapper;

        public DishCategoryService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IDishValidator dishValidator,
            IMapper mapper)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _dishValidator = dishValidator;
            _mapper = mapper;
        }

        public async Task AddDishesToCategory(AddDishesToCategoryParam param)
        {
            var category = await _readOnlyRepository.
                    GetFirstAsync<DishCategory>(dc => dc.Id == param.CategoryId);

            if (category == null)
            {
                throw new BadRequestException($"no such category found in db, category id: {param.CategoryId}");
            }

            await _dishValidator.ValidateDishIds(param.DishIds, param.RestaurantId);

            await _dishValidator.ValidateDuplicateIds(param.DishIds, param.RestaurantId, param.CategoryId);

            var maxIndex = _readOnlyRepository.GetAll<Dish_DishCategory>().
                            Where(ddc => ddc.DishCategoryId == param.CategoryId)
                            .Select(ddc => ddc?.Index)?.Max() ?? default(byte);

            var dish_DishCategories = new List<Dish_DishCategory>();

            foreach (var dishId in param.DishIds)
            {
                dish_DishCategories.Add(new Dish_DishCategory
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    DishId = dishId,
                    DishCategoryId = param.CategoryId,
                    RestaurantId = param.RestaurantId,
                    MenuId = category.MenuId,
                    IsOnShelf = true,
                    Index = ++maxIndex
                });
            }
            await _repository.CreateRangeAsync(dish_DishCategories);
        }

        public async Task<List<DishCategoryResult>> GetAllCategories(GetAllParam param)
        {
            if(param.MenuId.IsNullOrEmpty())
            {
                var defaultMenu = await _readOnlyRepository.GetFirstAsync<Menu>(m =>
                    m.IsDefault);

                param.MenuId = defaultMenu.Id;

                if (defaultMenu == null)
                {
                    var fakeDefaultMenu = await _readOnlyRepository.GetFirstAsync<Menu>();
                    param.MenuId = fakeDefaultMenu.Id;
                }
            }

            var categories = await _readOnlyRepository.GetAllAsync<DishCategory>(c =>
                c.MenuId == param.MenuId);

            if (categories == null || !categories.Any())
            {
                return new List<DishCategoryResult>();
            }

            var dishes = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(d => 
                d.MenuId == param.MenuId)).ToList();

            return categories.Select(c => new DishCategoryResult
            {
                Id = c.Id,
                Name = c.Name,
                Index = c.Index,
                CountOfDishes = dishes.Where(d => d.DishCategoryId == c.Id && d.IsOnShelf).Count()
            }).OrderBy(c => c.Index).ToList();
        }

        public async Task<DishesInCategoryResult> GetAllDishes(string dishCategoryId)
        {
            var result = new DishesInCategoryResult
            {
                CategoryId = dishCategoryId
            };

            var dishes = (await _readOnlyRepository.GetAllAsync<Dish_DishCategory>(ddc =>
                ddc.DishCategoryId == dishCategoryId, null, "Dish")).
                            Select(ddc => new DishResult
                            {
                                Id = ddc.DishId,
                                Name = ddc.Dish.Name,
                                UnitPrice = ddc.Dish.UnitPrice,
                                Index = ddc.Index,
                                Icon = ddc.Dish.Icon
                            }).ToList();
            result.Dishes = dishes;
            return result;
        }

        public async Task TransferDishes(TransferDishesParam param)
        {
            await _dishValidator.ValidateDishIds(param.DishIds, param.RestaurantId);

            var categoryIds = _readOnlyRepository.GetAll<DishCategory>().
                    Where(dc => dc.MenuId == param.MenuId).
                    Select(c => c.Id).ToList();

            categoryIds.ForEach(categoryId =>
            {
                DeleteDishesFromMenu(new DeleteDishesFromMenuParam
                {
                    RestaurantId = param.RestaurantId,
                    MenuId = param.MenuId,
                    DishIds = param.DishIds
                }).Wait();
            });

            await AddDishesToCategory(new AddDishesToCategoryParam
            {
                RestaurantId = param.RestaurantId,
                CategoryId = param.ToCategoryId,
                DishIds = param.DishIds
            });
        }

        public async Task DeleteDishesFromMenu(DeleteDishesFromMenuParam param)
        {
            var menu = await _readOnlyRepository.GetFirstAsync<Menu>(m => m.Id == param.MenuId);

            if (menu == null)
            {
                throw new BadRequestException($"no such menu in our db, menu id: {param.MenuId}");
            }

            await _dishValidator.ValidateDishIds(param.DishIds, param.RestaurantId);

            var allDish_DishCategories = _readOnlyRepository.GetAll<Dish_DishCategory>()
                .Where(ddc => ddc.MenuId == param.MenuId);

            var dish_DishCategories = param.DishIds.Join(allDish_DishCategories,
                dishId => dishId,
                ddc => ddc.DishId,
                (dishId, ddc) => ddc).ToList();

            _repository.DeleteRange(dish_DishCategories);
        }

        public Task UpdateCategoryIndexes(UpdateCategoryIndexesParam param)
        {
            var dishCategories = _readOnlyRepository.GetAll<DishCategory>().
                Where(ddc => ddc.MenuId == param.MenuId).ToList();
            if (dishCategories == null && !dishCategories.Any())
            {
                throw new BadRequestException($"no such menu in db with id: {param.MenuId}");
            }

            var categoryIdsInDb = dishCategories.Select(cat => cat.Id).ToList();

            var isBadRequest = param.CategoryIds.Any(id => !categoryIdsInDb.Contains(id));

            if (isBadRequest)
            {
                throw new BadRequestException($"check your ids please, there are some categories that not belongs to the menu you provided. ");
            }

            byte newIndex = 1;
            param.CategoryIds.ForEach(categoryId =>
            {
                var dishCategory = dishCategories.FirstOrDefault(ddc => ddc.Id == categoryId);
                dishCategory.Index = newIndex;
                newIndex++;
            });            
            _repository.UpdateRange(dishCategories);
            return Task.CompletedTask;
        }

        public async Task<List<GetAllDishCategoryResult>> GetAll(string menuId)
        {
            var dishCategories = await _repository.GetAllAsync<DishCategory>(dc =>
                dc.MenuId == menuId, categories => categories.OrderBy(d => d.Index));

            return dishCategories.Select(dc => new GetAllDishCategoryResult
            {
                Id = dc.Id,
                Name = dc.Name,
                Index = dc.Index
            }).ToList();
        }
    }
}
