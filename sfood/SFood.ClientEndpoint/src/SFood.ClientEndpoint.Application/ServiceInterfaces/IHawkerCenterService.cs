using SFood.ClientEndpoint.Application.Dtos.Parameter;
using SFood.ClientEndpoint.Application.Dtos.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceInterfaces
{
    public interface IHawkerCenterService
    {
        Task<IEnumerable<RestaurantResult>> GetRestaurants(GetRestaurantsParam param);

        Task<CenterRoughDto> GetCenterRough(GetCenterParam param);
    }
}
