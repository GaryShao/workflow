using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using SFood.MerchantEndpoint.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SFood.MerchantEndpoint.Application.Validator.Implements
{
    public class CustomizationValidator : ICustomizationValidator
    {
        public void ValidateCustomization(List<CustomizationCategoryDto> customizationCategories)
        {
            if (customizationCategories != null && customizationCategories.Any())
            {
                var singleCategories = customizationCategories.Where(cc => cc.MaxSelected == 1);
                if (singleCategories != null && singleCategories.Any())
                {
                    ValidateSingleCategory(singleCategories);
                }
                
                var multiCategories = customizationCategories.Where(cc => cc.MaxSelected > 1);
                if (multiCategories != null && multiCategories.Any())
                {
                    ValidateMultiCategory(multiCategories);
                }                
            }
        }

        private void ValidateSingleCategory(IEnumerable<CustomizationCategoryDto> singleCategories)
        {
            //单选的多配置： 只能由一个选项设置为default
            var isValid = singleCategories.All(sc => sc.Options != null &&
                sc.Options.Any() &&
                sc.Options.Count(op => op.IsDefault) == 1);

            if (!isValid)
            {
                throw new BadRequestException($"Customization can only contains one option which is set as default. ");
            }
        }

        private void ValidateMultiCategory(IEnumerable<CustomizationCategoryDto> multiCategories)
        {
            //多选的多配置： 只能由一个选项设置为default, default项的值必为0
            var isValid = multiCategories.All(sc => sc.Options != null &&
                sc.Options.Any() &&
                sc.Options.Count(op => op.IsDefault) == 1 &&
                sc.Options.First(op => op.IsDefault).UnitPrice == 0
            );

            if (!isValid)
            {
                throw new BadRequestException($"Customization can only contains one option which is set as default. And for the ones which can select multiple options, their default option's price must be 0");
            }
        }
    }
}
