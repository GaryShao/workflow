using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Validator
{
    public interface ICustomizationValidator
    {
        void ValidateCustomization(List<CustomizationCategoryDto> customizationCategories);
    }
}
