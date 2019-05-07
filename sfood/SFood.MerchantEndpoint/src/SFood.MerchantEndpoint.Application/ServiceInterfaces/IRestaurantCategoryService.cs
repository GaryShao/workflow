using SFood.MerchantEndpoint.Application.Dtos.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IRestaurantCategoryService
    {
        Task<List<RestaurantCategoryResult>> GetAll();
    }
}
