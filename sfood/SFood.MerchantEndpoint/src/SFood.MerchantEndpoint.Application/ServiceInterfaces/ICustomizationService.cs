using SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface ICustomizationService
    {
        Task<Dtos.Results.Customization.GetCategoriesResult> GetCustomizationCategories(string dishId, string restaurantId);

        Task<List<Dtos.Results.Customization.CategoryResult>> AddCustomizationCategoris(AddCustomizationsParam param);

        Task<List<Dtos.Results.Customization.CategoryResult>> UpdateCustomizationCategories(UpdateCustomizationsParam param);
    }
}
