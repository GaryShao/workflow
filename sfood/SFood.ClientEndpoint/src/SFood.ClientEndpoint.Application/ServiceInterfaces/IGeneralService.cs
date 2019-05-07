using SFood.ClientEndpoint.Application.Dtos.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceInterfaces
{
    public interface IGeneralService
    {
        Task<IEnumerable<RestaurantCategoryResult>> GetRestaurantCategories();

        Task<List<GroupedCountryCodesResult>> GetCountryCodes();
    }
}
