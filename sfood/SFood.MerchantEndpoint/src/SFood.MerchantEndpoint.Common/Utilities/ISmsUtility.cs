using SFood.MerchantEndpoint.Common.Dtos;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Utilities
{
    public interface ISmsUtility
    {
        Task SendingAsync(SendParam param);
    }
}
