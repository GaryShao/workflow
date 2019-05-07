using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization;
using SFood.MerchantEndpoint.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class CustomizationService : ICustomizationService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;

        public CustomizationService(IRepository repository
            , IReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }              

        public async Task<Dtos.Results.Customization.GetCategoriesResult> GetCustomizationCategories(string dishId, string restaurantId)
        {
            var result = new List<Dtos.Results.Customization.CategoryResult>();

            if (!dishId.IsNullOrEmpty())
            {
                var relatedCategories = await GetCustomizationCategories(dishId);
                if (relatedCategories == null || !relatedCategories.Any())
                {
                    return new Dtos.Results.Customization.GetCategoriesResult
                    {
                        DishId = dishId,
                        IsNew = true,
                        Categories = await GetSystemCustomizationCategories(restaurantId)
                    };
                }
                return new Dtos.Results.Customization.GetCategoriesResult
                {
                    DishId = dishId,
                    IsNew = false,
                    Categories = relatedCategories
                };
            }
            else
            {
                //first time to get list of customization categories
                return new Dtos.Results.Customization.GetCategoriesResult
                {
                    IsNew = true,
                    Categories = await GetSystemCustomizationCategories(restaurantId)
                };
            }
        }

        /// <summary>
        /// 新增规格时，保存规格列表，
        /// 此时进来的规格都是新增的(即便预设， 亦是新增； 但是预设值携带了id进来)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<Dtos.Results.Customization.CategoryResult>> AddCustomizationCategoris(AddCustomizationsParam param)
        {
            //此处要添加两种entity：
            //1. 要添加若干规格的分类
            //2. 每个规格都要添加若干规格值

            var systemCategoryIds = await GetSystemCustomizationCategoryIds(param.RestaurantId);
            //记录传递进来时分类的顺序
            byte index = 0;
            param.Categories.ForEach(c => {
                c.Index = index++;
            });

            param.Categories.Where(c => systemCategoryIds.Contains(c.Id)).ForEach(c => {
                c.FromId = c.Id;
                c.Id = null;
            });

            var addCategories = new List<CustomizationCategory>();
            param.Categories.ForEach(category => {
                var customizationCategory = new CustomizationCategory
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    Name = category.Name,
                    IsMultiple = category.IsMultiple,
                    IsSelected = category.IsSelected,
                    MaxOptions = category.MaxSelected,
                    FromId = category.FromId,
                    Index = category.Index,
                    IsSystem = false,
                    RestaurantId = param.RestaurantId
                };

                var addCustomizationOptions = new List<Customization>();
                byte optionIndex = 0;
                category.Options.ForEach(opt => {
                    var customization = new Customization
                    {
                        Id = Guid.NewGuid().ToUuidString(),
                        Name = opt.Name,
                        Index = optionIndex++,
                        UnitPrice = opt.UnitPrice,
                        IsDefault = opt.IsDefault,
                        CategoryId = customizationCategory.Id
                    };
                    addCustomizationOptions.Add(customization);
                });

                customizationCategory.Customizations = addCustomizationOptions;
                addCategories.Add(customizationCategory);
            });
            await _repository.CreateRangeAsync(addCategories);

            return addCategories.Select(c => new Dtos.Results.Customization.CategoryResult
            {
                Id = c.Id,
                Name = c.Name,                
                IsMultiple = c.IsMultiple,
                IsSelected = c.IsSelected,
                IsSystem = c.IsSystem,
                MaxSelected = c.MaxOptions,
                Options = c.Customizations.Select(x => new Dtos.Results.Customization.OptionResult {
                    Name = x.Name,
                    UnitPrice = x.UnitPrice.ToMoneyString(),
                    IsDefault = x.IsDefault
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// 修改规格时，保存规格列表，
        /// 此时进来的数据代表了3种状态：
        /// 1. 没有id 新增
        /// 2. 有id 编辑
        /// 3. 没出现的表示删除 (比对依据是根据dishId， 因为这时表示已经有)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<Dtos.Results.Customization.CategoryResult>> UpdateCustomizationCategories(UpdateCustomizationsParam param)
        {
            var systemCategoryIds = await GetSystemCustomizationCategoryIds(param.RestaurantId);
            //记录传递进来时分类的顺序
            byte index = 0;
            param.Categories.ForEach(c => {
                c.Index = index++;
            });

            param.Categories.Where(c => systemCategoryIds.Contains(c.Id)).ForEach(c => {
                c.FromId = c.Id;
                c.Id = null;
            });

            //1. categories to add
            var categoriesToAdd = param.Categories.Where(c => c.Id.IsNullOrEmpty());

            var addResult = await AddCustomizationCategoris(new AddCustomizationsParam {
                RestaurantId = param.RestaurantId,
                Categories = categoriesToAdd.ToList()
            });

            //2. categories to edit
            var categoriesToEdit = param.Categories.Where(c => !c.Id.IsNullOrEmpty()).ToList();

            await EditCustomizationCategoris(categoriesToEdit);

            var editResult = categoriesToEdit.Select(c => new Dtos.Results.Customization.CategoryResult {
                Id = c.Id,
                Name = c.Name,
                IsMultiple = c.IsMultiple,
                IsSelected = c.IsSelected,
                IsSystem = c.IsSystem,
                MaxSelected = c.MaxSelected,
                Options = c.Options.Select(x => new Dtos.Results.Customization.OptionResult {
                    Name = x.Name,
                    UnitPrice = x.UnitPrice.ToMoneyString(),
                    IsDefault = x.IsDefault
                }).ToList()
            }).ToList();

            //oldCategoryIds: 已经和菜品关联过的规格， 需要根据这个集合来判断， 用户删除了哪些规格
            var oldCategoryIds = (await _readOnlyRepository.GetAllAsync<Dish_CustomizationCategory>(dcc =>
                dcc.DishId == param.DishId)).Select(ddc => ddc.CustomizationCategoryId).ToList();            
            
            //3. categories to delete
            oldCategoryIds.RemoveAll(id => 
                param.Categories.Select(c => c.Id).Contains(id));

            await DeleteCustomizationCategoris(oldCategoryIds);

            var result = new List<Dtos.Results.Customization.CategoryResult>();
            result.AddRange(addResult);
            result.AddRange(editResult);
            return result;
        }

        /// <summary>
        /// 通过restaurantId找到其所属所有分类的所有规格
        /// </summary>
        /// <param name="restaurantId">餐厅id</param>
        private async Task<List<string>> GetSystemCustomizationCategoryIds(string restaurantId)
        {
            // 1. get all categories of a restaurant
            var restaurantCategoryIds = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(rrc => 
                rrc.RestaurantId == restaurantId)).Select(rc => rc.RestaurantCategoryId);

            //2. find all pre-defined system customization categories
            var result = (await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc => 
                restaurantCategoryIds.Contains(cc.RestaurantCategoryId))).Select(cc => cc.Id).ToList();
            return result;
        }        

        private async Task<List<Dtos.Results.Customization.CategoryResult>> GetSystemCustomizationCategories(string restaurantId)
        {
            // 1. get all categories of a restaurant
            var restaurantCategoryIds = (await _readOnlyRepository.GetAllAsync<Restaurant_RestaurantCategory>(rrc =>
                rrc.RestaurantId == restaurantId)).Select(rc => rc.RestaurantCategoryId);

            //2. find all pre-defined system customization categories
            var categories = await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc => cc.IsSystem &&
                restaurantCategoryIds.Contains(cc.RestaurantCategoryId), null, "Customizations");

            var result = new List<Dtos.Results.Customization.CategoryResult>();
            foreach (var category in categories)
            {
                var categoryResult = new Dtos.Results.Customization.CategoryResult
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSystem = category.IsSystem,
                    MaxSelected = category.MaxOptions,
                    IsMultiple = category.IsMultiple,
                    IsSelected = category.IsSelected
                };
                categoryResult.Options = category.Customizations.Select(c => new Dtos.Results.Customization.OptionResult {
                    Id = c.Id,
                    Name = c.Name,
                    Index = c.Index,
                    UnitPrice = c.UnitPrice.ToMoneyString(),
                    IsDefault = c.IsDefault
                }).ToList();

                result.Add(categoryResult);
            }
            return result; 
        }

        private async Task<List<Dtos.Results.Customization.CategoryResult>> GetCustomizationCategories(string dishId)
        {
            var categoryIds = (await _readOnlyRepository.GetAllAsync<Dish_CustomizationCategory>(dcc =>
                dcc.DishId == dishId)).Select(dcc => dcc.CustomizationCategoryId).ToList();

            var categories = await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc => 
                categoryIds.Contains(cc.Id), null, "Customizations");

            var result = new List<Dtos.Results.Customization.CategoryResult>();
            foreach (var category in categories)
            {
                var categoryResult = new Dtos.Results.Customization.CategoryResult
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSystem = category.IsSystem,
                    MaxSelected = category.MaxOptions,
                    IsMultiple = category.IsMultiple,
                    IsSelected = category.IsSelected
                };
                categoryResult.Options = category.Customizations.Select(c => new Dtos.Results.Customization.OptionResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Index = c.Index,
                    UnitPrice = c.UnitPrice.ToMoneyString(),
                    IsDefault = c.IsDefault
                }).ToList();

                result.Add(categoryResult);
            }
            return result;
        }

        /// <summary>
        /// 批量修改规格
        /// </summary>
        /// <returns></returns>
        private async Task EditCustomizationCategoris(List<CategoryParam> param)
        {
            var categories = (await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc =>
                param.Select(p => p.Id).Contains(cc.Id))).ToList();

            var categoryIds = categories.Select(c => c.Id).ToList();

            var oldOptions = (await _readOnlyRepository.GetAllAsync<Customization>(c => 
                categoryIds.Contains(c.Id))).ToList();

            var categoriesToEdit = new List<CustomizationCategory>();

            foreach (var category in param)
            {
                //记录传递进来时分类的顺序
                byte index = 0;
                category.Options.ForEach(c => {
                    c.Index = index++;
                });

                var categoryDb = categories.FirstOrDefault(c => c.Id == category.Id);
                if (GetCustomizationCategoryHashCode(categoryDb) != GetCustomizationCategoryParamHashCode(category))
                {
                    //分类本身的属性已经发生了变化
                    categoryDb.Name = category.Name;
                    categoryDb.Index = category.Index;
                    categoryDb.MaxOptions = category.MaxSelected;
                    categoryDb.IsMultiple = category.IsMultiple;
                    categoryDb.IsSelected = category.IsSelected;

                    categoriesToEdit.Add(categoryDb);
                }
                //检查分类下的规格值
                var oldOptionsForThisCategory = oldOptions.Where(o => o.CategoryId == category.Id).ToList();

                var options = category.Options;

                //1. add options
                var optionsToAdd = options.Where(o => o.Id.IsNullOrEmpty()).ToList();
                await AddCustomizationOptions(optionsToAdd, category.Id);


                //2. edit options
                var optionsToEdit = options.Where(o => !o.Id.IsNullOrEmpty()).ToList();
                await UpdateCustomizationOptions(optionsToEdit);

                //3. delete options
                var oldOptionIdsForThisCategory = oldOptionsForThisCategory.Select(opt => opt.Id).ToList();
                var optionIdsFromParam = options.Select(opt => opt.Id);
                oldOptionIdsForThisCategory.RemoveAll(o => optionIdsFromParam.Contains(o));

                await DeleteCustomizationOptions(oldOptionIdsForThisCategory);
            }
            _repository.UpdateRange(categoriesToEdit);
        }

        /// <summary>
        /// 批量删除规格分类
        /// </summary>
        /// <returns></returns>
        private async Task DeleteCustomizationCategoris(List<string> ids)
        {
            var categories = await _readOnlyRepository.GetAllAsync<CustomizationCategory>(cc =>
                ids.Contains(cc.Id));
            _repository.DeleteRange(categories.ToList());
        }

        private async Task AddCustomizationOptions(List<OptionParam> param, string categoryId)
        {
            var customizations = new List<Customization>();
            param.ForEach(option => {
                var customization = new Customization {
                    Id = Guid.NewGuid().ToUuidString(),
                    Name = option.Name,
                    Index = option.Index,
                    UnitPrice = option.UnitPrice,
                    IsDefault = option.IsDefault
                };
                customizations.Add(customization);
            });

            await _repository.CreateRangeAsync(customizations);
        }

        private async Task UpdateCustomizationOptions(List<OptionParam> param)
        {
            var customizationIds = param.Select(p => p.Id).ToList();
            var customizations = await _readOnlyRepository.GetAllAsync<Customization>(c => customizationIds.Contains(c.Id));
            customizations.ForEach(custom => {
                var option = param.FirstOrDefault(p => p.Id == custom.Id);
                custom.Name = option.Name;
                custom.Index = option.Index;
                custom.UnitPrice = option.UnitPrice;
                custom.IsDefault = option.IsDefault;
            });

            _repository.UpdateRange(customizations.ToList());
        }

        private async Task DeleteCustomizationOptions(List<string> optionIds)
        {
            var options = await _readOnlyRepository.GetAllAsync<Customization>(c =>
                optionIds.Contains(c.Id));
            _repository.DeleteRange(options.ToList());
        }

        private int GetCustomizationCategoryHashCode(CustomizationCategory category)
        {
            return category.Name.GetHashCode() + category.Index.GetHashCode() +
                category.MaxOptions.GetHashCode() + category.IsMultiple.GetHashCode() +
                category.IsSelected.GetHashCode();
        }

        private int GetCustomizationCategoryParamHashCode(CategoryParam category)
        {
            return category.Name.GetHashCode() + category.Index.GetHashCode() + 
                category.MaxSelected.GetHashCode() + category.IsMultiple.GetHashCode() +
                category.IsSelected.GetHashCode();
        }       
    }
}
