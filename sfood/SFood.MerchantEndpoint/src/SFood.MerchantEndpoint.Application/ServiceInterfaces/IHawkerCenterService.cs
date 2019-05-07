using SFood.MerchantEndpoint.Application.Dtos.Results.HawkerCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IHawkerCenterService
    {
        Task<List<HawkerCenterResult>> GetAll();
    }
}
